using AppWindows.Data;
using AppWindows.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel;

namespace AppWindows;

public sealed partial class MainWindow : Window
{
    private readonly DatabaseService databaseService;
    private readonly StudentRepository studentRepository;
    private readonly ObservableCollection<Student> students = [];
    private readonly ObservableCollection<Tag> filterTags = [];
    private readonly List<Tag> availableTags = [];
    private readonly Dictionary<string, CheckBox> tagCheckBoxes = [];

    private Student? selectedStudent;

    public MainWindow()
    {
        InitializeComponent();

        databaseService = new DatabaseService();
        databaseService.Initialize();
        studentRepository = new StudentRepository(databaseService);

        StudentsListView.ItemsSource = students;
        FilterTagComboBox.ItemsSource = filterTags;

        LoadTags();
        ResetEditor();
        RefreshStudents();
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

    private void AddTagButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            var tag = studentRepository.CreateCustomTag(CustomTagTextBox.Text);
            CustomTagTextBox.Text = string.Empty;
            LoadTags((FilterTagComboBox.SelectedItem as Tag)?.Id);

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

    private void CancelEditButton_Click(object sender, RoutedEventArgs e)
    {
        ResetEditor();
    }

    private void StudentsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (StudentsListView.SelectedItem is Student student)
        {
            SelectStudent(student);
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

    private void ShowStatus(string message, InfoBarSeverity severity)
    {
        StatusInfoBar.Message = message;
        StatusInfoBar.Severity = severity;
        StatusInfoBar.IsOpen = true;
    }
}
