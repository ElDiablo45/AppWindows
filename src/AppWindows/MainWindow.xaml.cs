using AppWindows.Data;
using AppWindows.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System.Collections.ObjectModel;

namespace AppWindows;

public sealed partial class MainWindow : Window
{
    private readonly DatabaseService databaseService;
    private readonly StudentRepository studentRepository;
    private readonly InvoiceRepository invoiceRepository;
    private readonly ObservableCollection<Student> students = [];
    private readonly ObservableCollection<Student> recentStudents = [];
    private readonly ObservableCollection<Tag> filterTags = [];
    private readonly ObservableCollection<Invoice> invoices = [];
    private readonly ObservableCollection<Student> invoiceClients = [];
    private readonly ObservableCollection<InvoiceTemplate> invoiceTemplates = [];
    private readonly ObservableCollection<InvoiceStatusOption> invoiceStatusOptions = [];
    private readonly ObservableCollection<InvoiceStatusOption> invoiceStatusFilterOptions = [];
    private readonly List<Tag> availableTags = [];
    private readonly Dictionary<string, CheckBox> tagCheckBoxes = [];

    private Student? selectedStudent;
    private Invoice? selectedInvoice;
    private bool isLoadingInvoiceEditor;

    public MainWindow()
    {
        InitializeComponent();

        databaseService = new DatabaseService();
        databaseService.Initialize();
        studentRepository = new StudentRepository(databaseService);
        invoiceRepository = new InvoiceRepository(databaseService);

        StudentsListView.ItemsSource = students;
        RecentStudentsListView.ItemsSource = recentStudents;
        FilterTagComboBox.ItemsSource = filterTags;
        InvoicesListView.ItemsSource = invoices;
        InvoiceClientComboBox.ItemsSource = invoiceClients;
        InvoiceTemplateComboBox.ItemsSource = invoiceTemplates;
        InvoiceStatusComboBox.ItemsSource = invoiceStatusOptions;
        InvoiceStatusFilterComboBox.ItemsSource = invoiceStatusFilterOptions;

        LoadInvoiceStatusOptions();
        LoadTags();
        LoadInvoiceReferenceData();
        ResetEditor();
        ResetInvoiceEditor();
        RefreshStudents();
        RefreshInvoices();
        ShowHomeView();
    }

    private void LoadTags(string? selectedTagId = null)
    {
        availableTags.Clear();
        availableTags.AddRange(studentRepository.GetTags());

        filterTags.Clear();
        filterTags.Add(new Tag { Id = string.Empty, Name = "Todos", IsPreset = true });
        foreach (var tag in availableTags)
        {
            filterTags.Add(tag);
        }

        FilterTagComboBox.SelectedItem = filterTags.FirstOrDefault(tag => tag.Id == selectedTagId) ?? filterTags[0];
        RenderTagCheckBoxes();
    }

    private void LoadInvoiceStatusOptions()
    {
        invoiceStatusOptions.Clear();
        foreach (var option in InvoiceStatuses.Options)
        {
            invoiceStatusOptions.Add(option);
        }

        invoiceStatusFilterOptions.Clear();
        invoiceStatusFilterOptions.Add(new InvoiceStatusOption(string.Empty, "Todos"));
        foreach (var option in InvoiceStatuses.Options)
        {
            invoiceStatusFilterOptions.Add(option);
        }

        InvoiceStatusComboBox.SelectedItem = invoiceStatusOptions.First(option => option.Id == InvoiceStatuses.Draft);
        InvoiceStatusFilterComboBox.SelectedItem = invoiceStatusFilterOptions[0];
    }

    private void LoadInvoiceReferenceData(string? selectedClientId = null, string? selectedTemplateId = null)
    {
        isLoadingInvoiceEditor = true;

        var clientId = selectedClientId ?? (InvoiceClientComboBox.SelectedItem as Student)?.Id;
        invoiceClients.Clear();
        foreach (var student in studentRepository.GetStudents(null, null))
        {
            invoiceClients.Add(student);
        }

        InvoiceClientComboBox.SelectedItem = invoiceClients.FirstOrDefault(student => student.Id == clientId)
            ?? invoiceClients.FirstOrDefault();

        var templateId = selectedTemplateId ?? (InvoiceTemplateComboBox.SelectedItem as InvoiceTemplate)?.Id;
        invoiceTemplates.Clear();
        invoiceTemplates.Add(new InvoiceTemplate
        {
            Id = string.Empty,
            Name = "Sin plantilla",
            CreatedAt = DateTime.Today
        });

        foreach (var template in invoiceRepository.GetTemplates())
        {
            invoiceTemplates.Add(template);
        }

        InvoiceTemplateComboBox.SelectedItem = invoiceTemplates.FirstOrDefault(template => template.Id == templateId)
            ?? invoiceTemplates[0];

        isLoadingInvoiceEditor = false;
    }

    private void RefreshStudents()
    {
        var currentStudentId = selectedStudent?.Id;
        var filterTag = FilterTagComboBox.SelectedItem as Tag;
        var loadedStudents = studentRepository.GetStudents(SearchTextBox.Text, filterTag?.Id);

        students.Clear();
        foreach (var student in loadedStudents)
        {
            students.Add(student);
        }

        StudentCountTextBlock.Text = $"{students.Count} alumnos";

        var matchingSelection = students.FirstOrDefault(student => student.Id == currentStudentId);
        if (matchingSelection is not null)
        {
            StudentsListView.SelectedItem = matchingSelection;
        }
        else if (selectedStudent is not null && students.Count == 0)
        {
            ResetEditor();
        }
    }

    private void RefreshInvoices()
    {
        var currentInvoiceId = selectedInvoice?.Id;
        var filterStatus = InvoiceStatusFilterComboBox.SelectedItem as InvoiceStatusOption;
        var loadedInvoices = invoiceRepository.GetInvoices(InvoiceSearchTextBox.Text, filterStatus?.Id);

        invoices.Clear();
        foreach (var invoice in loadedInvoices)
        {
            invoices.Add(invoice);
        }

        InvoiceCountTextBlock.Text = $"{invoices.Count} facturas";

        var matchingSelection = invoices.FirstOrDefault(invoice => invoice.Id == currentInvoiceId);
        if (matchingSelection is not null)
        {
            InvoicesListView.SelectedItem = matchingSelection;
        }
        else if (selectedInvoice is not null && invoices.Count == 0)
        {
            ResetInvoiceEditor();
        }
    }

    private void RefreshHomeDashboard()
    {
        HomeStudentTotalTextBlock.Text = studentRepository.GetStudentCount().ToString();
        HomeTagTotalTextBlock.Text = studentRepository.GetTagCount().ToString();

        recentStudents.Clear();
        foreach (var student in studentRepository.GetRecentStudents(4))
        {
            recentStudents.Add(student);
        }

        var hasRecentStudents = recentStudents.Count > 0;
        RecentStudentsListView.Visibility = hasRecentStudents ? Visibility.Visible : Visibility.Collapsed;
        RecentStudentsEmptyTextBlock.Visibility = hasRecentStudents ? Visibility.Collapsed : Visibility.Visible;
    }

    private void ShowHomeView()
    {
        HomeView.Visibility = Visibility.Visible;
        StudentsView.Visibility = Visibility.Collapsed;
        InvoicesView.Visibility = Visibility.Collapsed;
        PageTitleTextBlock.Text = "Inicio";
        PageSubtitleTextBlock.Text = "Resumen principal de la autoescuela y actividad reciente.";
        RefreshHomeDashboard();
        SetNavigationState("home");
    }

    private void ShowStudentsView(bool startNewStudent)
    {
        HomeView.Visibility = Visibility.Collapsed;
        StudentsView.Visibility = Visibility.Visible;
        InvoicesView.Visibility = Visibility.Collapsed;
        PageTitleTextBlock.Text = "Alumnos";
        PageSubtitleTextBlock.Text = "Panel de busqueda y gestion de alumnos de la autoescuela.";
        RefreshStudents();
        SetNavigationState("students");

        if (startNewStudent)
        {
            ResetEditor();
            FullNameTextBox.Focus(FocusState.Programmatic);
        }
    }

    private void ShowInvoicesView(bool startNewInvoice)
    {
        HomeView.Visibility = Visibility.Collapsed;
        StudentsView.Visibility = Visibility.Collapsed;
        InvoicesView.Visibility = Visibility.Visible;
        PageTitleTextBlock.Text = "Facturas";
        PageSubtitleTextBlock.Text = "Plantillas rapidas, cliente vinculado y facturas personalizadas.";
        LoadInvoiceReferenceData();
        RefreshInvoices();
        SetNavigationState("invoices");

        if (startNewInvoice)
        {
            ResetInvoiceEditor();
            InvoiceNumberTextBox.Focus(FocusState.Programmatic);
        }
    }

    private void SetNavigationState(string activeView)
    {
        var activeBrush = GetBrush("NavActiveBrush");
        var inactiveBrush = GetBrush("TransparentBrush");
        var activeStrokeBrush = GetBrush("AccentGoldBrush");

        HomeNavButton.Background = activeView == "home" ? activeBrush : inactiveBrush;
        HomeNavButton.BorderBrush = activeView == "home" ? activeStrokeBrush : inactiveBrush;
        StudentsNavButton.Background = activeView == "students" ? activeBrush : inactiveBrush;
        StudentsNavButton.BorderBrush = activeView == "students" ? activeStrokeBrush : inactiveBrush;
        InvoicesNavButton.Background = activeView == "invoices" ? activeBrush : inactiveBrush;
        InvoicesNavButton.BorderBrush = activeView == "invoices" ? activeStrokeBrush : inactiveBrush;
    }

    private Brush GetBrush(string key)
    {
        return (Brush)RootGrid.Resources[key];
    }

    private void RenderTagCheckBoxes()
    {
        tagCheckBoxes.Clear();
        TagCheckBoxesPanel.Children.Clear();

        foreach (var tag in availableTags)
        {
            var checkBox = new CheckBox
            {
                Content = tag.Name,
                Tag = tag.Id,
                MinWidth = 58
            };

            tagCheckBoxes[tag.Id] = checkBox;
            TagCheckBoxesPanel.Children.Add(checkBox);
        }

        ApplySelectedTagsToEditor();
    }

    private void ApplySelectedTagsToEditor()
    {
        var selectedTagIds = selectedStudent?.Tags.Select(tag => tag.Id).ToHashSet(StringComparer.OrdinalIgnoreCase)
            ?? [];

        foreach (var (tagId, checkBox) in tagCheckBoxes)
        {
            checkBox.IsChecked = selectedTagIds.Contains(tagId);
        }
    }

    private void SelectStudent(Student student)
    {
        selectedStudent = student;
        EditorTitleTextBlock.Text = student.FullName;
        EditorSubtitleTextBlock.Text = "Editando alumno existente";
        FullNameTextBox.Text = student.FullName;
        DniTextBox.Text = student.DniNie;
        PhoneTextBox.Text = student.Phone;
        NotesTextBox.Text = student.Notes;
        CreatedAtTextBlock.Text = $"Alta: {student.CreatedAtDisplay}";
        CancelEditButton.Content = "Nuevo";
        ApplySelectedTagsToEditor();
    }

    private void SelectInvoice(Invoice invoice)
    {
        selectedInvoice = invoice;
        isLoadingInvoiceEditor = true;

        InvoiceEditorTitleTextBlock.Text = invoice.Number;
        InvoiceEditorSubtitleTextBlock.Text = $"Factura para {invoice.ClientName}";
        InvoiceNumberTextBox.Text = invoice.Number;
        InvoiceClientComboBox.SelectedItem = invoiceClients.FirstOrDefault(student => student.Id == invoice.StudentId);
        InvoiceTemplateComboBox.SelectedItem = invoiceTemplates.FirstOrDefault(template => template.Id == invoice.TemplateId)
            ?? invoiceTemplates[0];
        InvoiceIssueDatePicker.Date = new DateTimeOffset(invoice.IssueDate);
        InvoiceDueDatePicker.Date = invoice.DueDate is null ? null : new DateTimeOffset(invoice.DueDate.Value);
        InvoiceConceptTextBox.Text = invoice.Concept;
        InvoiceAmountNumberBox.Value = Convert.ToDouble(invoice.Amount);
        InvoiceTaxNumberBox.Value = Convert.ToDouble(invoice.TaxRate);
        InvoiceTemplateNameTextBox.Text = invoice.TemplateName;
        InvoiceNotesTextBox.Text = invoice.Notes;
        InvoiceStatusComboBox.SelectedItem = invoiceStatusOptions.FirstOrDefault(option => option.Id == invoice.Status)
            ?? invoiceStatusOptions.First(option => option.Id == InvoiceStatuses.Draft);
        CancelInvoiceEditButton.Content = "Nueva";

        isLoadingInvoiceEditor = false;
        UpdateInvoicePreview();
    }

    private void ResetEditor()
    {
        selectedStudent = null;
        StudentsListView.SelectedItem = null;
        EditorTitleTextBlock.Text = "Nuevo alumno";
        EditorSubtitleTextBlock.Text = "Nombre, DNI/NIE y telefono son obligatorios";
        FullNameTextBox.Text = string.Empty;
        DniTextBox.Text = string.Empty;
        PhoneTextBox.Text = string.Empty;
        NotesTextBox.Text = string.Empty;
        CreatedAtTextBlock.Text = $"Alta: {DateTime.Today:dd/MM/yyyy}";
        CustomTagTextBox.Text = string.Empty;
        CancelEditButton.Content = "Limpiar";
        ApplySelectedTagsToEditor();
    }

    private void ResetInvoiceEditor()
    {
        selectedInvoice = null;
        InvoicesListView.SelectedItem = null;
        isLoadingInvoiceEditor = true;

        InvoiceEditorTitleTextBlock.Text = "Nueva factura";
        InvoiceEditorSubtitleTextBlock.Text = "Cliente, numero y concepto son obligatorios";
        InvoiceNumberTextBox.Text = invoiceRepository.GetNextInvoiceNumber();
        InvoiceClientComboBox.SelectedItem = invoiceClients.FirstOrDefault();
        InvoiceTemplateComboBox.SelectedItem = invoiceTemplates.FirstOrDefault();
        InvoiceIssueDatePicker.Date = new DateTimeOffset(DateTime.Today);
        InvoiceDueDatePicker.Date = new DateTimeOffset(DateTime.Today.AddDays(15));
        InvoiceConceptTextBox.Text = string.Empty;
        InvoiceAmountNumberBox.Value = 0;
        InvoiceTaxNumberBox.Value = 21;
        InvoiceTemplateNameTextBox.Text = string.Empty;
        InvoiceNotesTextBox.Text = string.Empty;
        InvoiceStatusComboBox.SelectedItem = invoiceStatusOptions.First(option => option.Id == InvoiceStatuses.Draft);
        CancelInvoiceEditButton.Content = "Limpiar";

        isLoadingInvoiceEditor = false;
        UpdateInvoicePreview();
    }

    private IReadOnlyCollection<string> GetSelectedTagIds()
    {
        return tagCheckBoxes
            .Where(pair => pair.Value.IsChecked == true)
            .Select(pair => pair.Key)
            .ToList();
    }

    private bool ValidateEditor()
    {
        if (string.IsNullOrWhiteSpace(FullNameTextBox.Text))
        {
            ShowStatus("El nombre es obligatorio.", InfoBarSeverity.Error);
            FullNameTextBox.Focus(FocusState.Programmatic);
            return false;
        }

        if (string.IsNullOrWhiteSpace(DniTextBox.Text))
        {
            ShowStatus("El DNI/NIE es obligatorio.", InfoBarSeverity.Error);
            DniTextBox.Focus(FocusState.Programmatic);
            return false;
        }

        if (string.IsNullOrWhiteSpace(PhoneTextBox.Text))
        {
            ShowStatus("El telefono es obligatorio.", InfoBarSeverity.Error);
            PhoneTextBox.Focus(FocusState.Programmatic);
            return false;
        }

        return true;
    }

    private bool ValidateInvoiceEditor()
    {
        if (string.IsNullOrWhiteSpace(InvoiceNumberTextBox.Text))
        {
            ShowStatus("El numero de factura es obligatorio.", InfoBarSeverity.Error);
            InvoiceNumberTextBox.Focus(FocusState.Programmatic);
            return false;
        }

        if (InvoiceClientComboBox.SelectedItem is not Student)
        {
            ShowStatus("Selecciona un cliente antes de crear la factura.", InfoBarSeverity.Error);
            InvoiceClientComboBox.Focus(FocusState.Programmatic);
            return false;
        }

        if (string.IsNullOrWhiteSpace(InvoiceConceptTextBox.Text))
        {
            ShowStatus("El concepto es obligatorio.", InfoBarSeverity.Error);
            InvoiceConceptTextBox.Focus(FocusState.Programmatic);
            return false;
        }

        if (GetInvoiceAmount() <= 0)
        {
            ShowStatus("El importe debe ser mayor que cero.", InfoBarSeverity.Error);
            InvoiceAmountNumberBox.Focus(FocusState.Programmatic);
            return false;
        }

        return true;
    }

    private void SaveStudentButton_Click(object sender, RoutedEventArgs e)
    {
        if (!ValidateEditor())
        {
            return;
        }

        try
        {
            var selectedTagIds = GetSelectedTagIds();
            if (selectedStudent is null)
            {
                selectedStudent = studentRepository.CreateStudent(
                    FullNameTextBox.Text,
                    DniTextBox.Text,
                    PhoneTextBox.Text,
                    NotesTextBox.Text,
                    selectedTagIds);
                ShowStatus("Alumno creado correctamente.", InfoBarSeverity.Success);
            }
            else
            {
                selectedStudent.FullName = FullNameTextBox.Text;
                selectedStudent.DniNie = DniTextBox.Text;
                selectedStudent.Phone = PhoneTextBox.Text;
                selectedStudent.Notes = NotesTextBox.Text;
                studentRepository.UpdateStudent(selectedStudent, selectedTagIds);
                ShowStatus("Alumno actualizado correctamente.", InfoBarSeverity.Success);
            }

            var savedStudentId = selectedStudent.Id;
            RefreshStudents();
            LoadInvoiceReferenceData(savedStudentId);
            RefreshHomeDashboard();
            var refreshedStudent = students.FirstOrDefault(student => student.Id == savedStudentId);
            if (refreshedStudent is not null)
            {
                StudentsListView.SelectedItem = refreshedStudent;
                SelectStudent(refreshedStudent);
            }
            else
            {
                ResetEditor();
                ShowStatus("Alumno guardado. No aparece en la lista por el filtro actual.", InfoBarSeverity.Informational);
            }
        }
        catch (DuplicateDniException exception)
        {
            ShowStatus(exception.Message, InfoBarSeverity.Error);
            DniTextBox.Focus(FocusState.Programmatic);
        }
        catch (Exception exception)
        {
            ShowStatus($"No se pudo guardar el alumno: {exception.Message}", InfoBarSeverity.Error);
        }
    }

    private void SaveInvoiceButton_Click(object sender, RoutedEventArgs e)
    {
        if (!ValidateInvoiceEditor())
        {
            return;
        }

        try
        {
            var client = (Student)InvoiceClientComboBox.SelectedItem;
            var template = InvoiceTemplateComboBox.SelectedItem as InvoiceTemplate;
            var status = (InvoiceStatusComboBox.SelectedItem as InvoiceStatusOption)?.Id ?? InvoiceStatuses.Draft;

            if (selectedInvoice is null)
            {
                selectedInvoice = invoiceRepository.CreateInvoice(
                    InvoiceNumberTextBox.Text,
                    client.Id,
                    template?.Id,
                    GetInvoiceIssueDate(),
                    GetInvoiceDueDate(),
                    InvoiceConceptTextBox.Text,
                    GetInvoiceAmount(),
                    GetInvoiceTaxRate(),
                    InvoiceNotesTextBox.Text,
                    status);
                ShowStatus("Factura creada correctamente.", InfoBarSeverity.Success);
            }
            else
            {
                selectedInvoice.Number = InvoiceNumberTextBox.Text;
                selectedInvoice.StudentId = client.Id;
                selectedInvoice.TemplateId = template?.Id;
                selectedInvoice.IssueDate = GetInvoiceIssueDate();
                selectedInvoice.DueDate = GetInvoiceDueDate();
                selectedInvoice.Concept = InvoiceConceptTextBox.Text;
                selectedInvoice.Amount = GetInvoiceAmount();
                selectedInvoice.TaxRate = GetInvoiceTaxRate();
                selectedInvoice.Notes = InvoiceNotesTextBox.Text;
                selectedInvoice.Status = status;
                invoiceRepository.UpdateInvoice(selectedInvoice);
                ShowStatus("Factura actualizada correctamente.", InfoBarSeverity.Success);
            }

            var savedInvoiceId = selectedInvoice.Id;
            RefreshInvoices();
            var refreshedInvoice = invoices.FirstOrDefault(invoice => invoice.Id == savedInvoiceId);
            if (refreshedInvoice is not null)
            {
                InvoicesListView.SelectedItem = refreshedInvoice;
                SelectInvoice(refreshedInvoice);
            }
            else
            {
                ResetInvoiceEditor();
                ShowStatus("Factura guardada. No aparece en la lista por el filtro actual.", InfoBarSeverity.Informational);
            }
        }
        catch (DuplicateInvoiceNumberException exception)
        {
            ShowStatus(exception.Message, InfoBarSeverity.Error);
            InvoiceNumberTextBox.Focus(FocusState.Programmatic);
        }
        catch (Exception exception)
        {
            ShowStatus($"No se pudo guardar la factura: {exception.Message}", InfoBarSeverity.Error);
        }
    }

    private void SaveInvoiceTemplateButton_Click(object sender, RoutedEventArgs e)
    {
        var templateName = InvoiceTemplateNameTextBox.Text.Trim();
        if (string.IsNullOrWhiteSpace(templateName))
        {
            ShowStatus("Escribe un nombre de plantilla antes de guardarla.", InfoBarSeverity.Error);
            InvoiceTemplateNameTextBox.Focus(FocusState.Programmatic);
            return;
        }

        if (string.IsNullOrWhiteSpace(InvoiceConceptTextBox.Text))
        {
            ShowStatus("La plantilla necesita un concepto.", InfoBarSeverity.Error);
            InvoiceConceptTextBox.Focus(FocusState.Programmatic);
            return;
        }

        try
        {
            var template = invoiceRepository.CreateTemplate(
                templateName,
                InvoiceConceptTextBox.Text,
                GetInvoiceAmount(),
                GetInvoiceTaxRate(),
                InvoiceNotesTextBox.Text);

            LoadInvoiceReferenceData((InvoiceClientComboBox.SelectedItem as Student)?.Id, template.Id);
            ShowStatus($"Plantilla {template.Name} guardada.", InfoBarSeverity.Success);
        }
        catch (Exception exception)
        {
            ShowStatus($"No se pudo guardar la plantilla: {exception.Message}", InfoBarSeverity.Error);
        }
    }

    private void AddTagButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            var tag = studentRepository.CreateCustomTag(CustomTagTextBox.Text);
            CustomTagTextBox.Text = string.Empty;
            LoadTags((FilterTagComboBox.SelectedItem as Tag)?.Id);
            RefreshHomeDashboard();

            if (tagCheckBoxes.TryGetValue(tag.Id, out var checkBox))
            {
                checkBox.IsChecked = true;
            }

            ShowStatus($"Tag {tag.Name} disponible.", InfoBarSeverity.Success);
        }
        catch (Exception exception)
        {
            ShowStatus(exception.Message, InfoBarSeverity.Error);
        }
    }

    private void NewStudentButton_Click(object sender, RoutedEventArgs e)
    {
        ResetEditor();
        FullNameTextBox.Focus(FocusState.Programmatic);
    }

    private void NewInvoiceButton_Click(object sender, RoutedEventArgs e)
    {
        ResetInvoiceEditor();
        InvoiceNumberTextBox.Focus(FocusState.Programmatic);
    }

    private void HomeNavButton_Click(object sender, RoutedEventArgs e)
    {
        ShowHomeView();
    }

    private void StudentsNavButton_Click(object sender, RoutedEventArgs e)
    {
        ShowStudentsView(startNewStudent: false);
    }

    private void InvoicesNavButton_Click(object sender, RoutedEventArgs e)
    {
        ShowInvoicesView(startNewInvoice: false);
    }

    private void QuickStudentButton_Click(object sender, RoutedEventArgs e)
    {
        ShowStudentsView(startNewStudent: true);
    }

    private void QuickInvoiceButton_Click(object sender, RoutedEventArgs e)
    {
        ShowInvoicesView(startNewInvoice: true);
    }

    private void QuickNoteButton_Click(object sender, RoutedEventArgs e)
    {
        ShowStatus("El modulo de notas esta preparado para el siguiente paso.", InfoBarSeverity.Informational);
    }

    private void RecentStudentsListView_ItemClick(object sender, ItemClickEventArgs e)
    {
        if (e.ClickedItem is not Student clickedStudent)
        {
            return;
        }

        ShowStudentsView(startNewStudent: false);
        var matchingStudent = students.FirstOrDefault(student => student.Id == clickedStudent.Id);
        if (matchingStudent is not null)
        {
            StudentsListView.SelectedItem = matchingStudent;
            SelectStudent(matchingStudent);
        }
    }

    private void CancelEditButton_Click(object sender, RoutedEventArgs e)
    {
        ResetEditor();
    }

    private void CancelInvoiceEditButton_Click(object sender, RoutedEventArgs e)
    {
        ResetInvoiceEditor();
    }

    private void StudentsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (StudentsListView.SelectedItem is Student student)
        {
            SelectStudent(student);
        }
    }

    private void InvoicesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (InvoicesListView.SelectedItem is Invoice invoice)
        {
            SelectInvoice(invoice);
        }
    }

    private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        RefreshStudents();
    }

    private void FilterTagComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        RefreshStudents();
    }

    private void InvoiceSearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        RefreshInvoices();
    }

    private void InvoiceStatusFilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        RefreshInvoices();
    }

    private void InvoiceTemplateComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (isLoadingInvoiceEditor || InvoiceTemplateComboBox.SelectedItem is not InvoiceTemplate template || string.IsNullOrWhiteSpace(template.Id))
        {
            return;
        }

        InvoiceConceptTextBox.Text = template.Concept;
        InvoiceAmountNumberBox.Value = Convert.ToDouble(template.Amount);
        InvoiceTaxNumberBox.Value = Convert.ToDouble(template.TaxRate);
        InvoiceTemplateNameTextBox.Text = template.Name;
        InvoiceNotesTextBox.Text = template.Notes;
        UpdateInvoicePreview();
    }

    private void InvoiceAmountNumberBox_ValueChanged(NumberBox sender, NumberBoxValueChangedEventArgs args)
    {
        UpdateInvoicePreview();
    }

    private void UpdateInvoicePreview()
    {
        var amount = GetInvoiceAmount();
        var tax = Math.Round(amount * GetInvoiceTaxRate() / 100m, 2);
        InvoiceTotalPreviewTextBlock.Text = (amount + tax).ToString("C");
    }

    private DateTime GetInvoiceIssueDate()
    {
        return InvoiceIssueDatePicker.Date?.DateTime.Date ?? DateTime.Today;
    }

    private DateTime? GetInvoiceDueDate()
    {
        return InvoiceDueDatePicker.Date?.DateTime.Date;
    }

    private decimal GetInvoiceAmount()
    {
        return GetNumberBoxDecimal(InvoiceAmountNumberBox);
    }

    private decimal GetInvoiceTaxRate()
    {
        return GetNumberBoxDecimal(InvoiceTaxNumberBox);
    }

    private static decimal GetNumberBoxDecimal(NumberBox numberBox)
    {
        return double.IsNaN(numberBox.Value) ? 0m : Convert.ToDecimal(numberBox.Value);
    }

    private void ShowStatus(string message, InfoBarSeverity severity)
    {
        StatusInfoBar.Message = message;
        StatusInfoBar.Severity = severity;
        StatusInfoBar.IsOpen = true;
    }
}
