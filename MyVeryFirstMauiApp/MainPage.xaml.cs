using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace MyVeryFirstMauiApp;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();

        var vm = this.BindingContext as TestViewModel;
        vm.CreateTestListItems();
    }

    private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
    {
        var grid = (Grid)sender;

        var item = (TestCard)grid.BindingContext;
        item.IsTicked = true;
    }
}

public class TestViewModel : INotifyPropertyChanged
{
    private ObservableCollection<TestCard> testList;
    public ObservableCollection<TestCard> TestListCollection
    {
        get { return testList; }
        set { this.testList = value; }
    }

    public TestViewModel()
    {
        testList = new ObservableCollection<TestCard>();
    }

    public void CreateTestListItems()
    {
        testList.Add(new TestCard("Jay"));
        testList.Add(new TestCard("Blackbird"));
        testList.Add(new TestCard("Thrush"));
        testList.Add(new TestCard("Sparrowhawk"));
        testList.Add(new TestCard("Pigeon"));
    }

    protected bool SetProperty<T>(ref T backingStore, T value,
        [CallerMemberName] string propertyName = "",
        Action onChanged = null)
    {
        if (EqualityComparer<T>.Default.Equals(backingStore, value))
            return false;

        backingStore = value;
        onChanged?.Invoke();
        OnPropertyChanged(propertyName);
        return true;
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
    {
        var changed = PropertyChanged;
        if (changed == null)
            return;

        changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

public class TestCard : INotifyPropertyChanged
{
    public TestCard(string name)
    {
        this.Name = name;
    }

    private string name;
    public string Name
    {
        get
        {
            return name;
        }
        set
        {
            SetProperty(ref name, value);
        }
    }

    private bool isTicked;
    public bool IsTicked
    {
        get
        {
            return isTicked;
        }
        set
        {
            SetProperty(ref isTicked, value);

            // Other properties may change as a result of this.
            OnPropertyChanged("AccessibleName");
        }
    }

    private string accessibleName;
    public string AccessibleName
    {
        get
        {
            return name + (isTicked ? " Ticked!" : "");
        }
    }

    protected bool SetProperty<T>(ref T backingStore, T value,
        [CallerMemberName] string propertyName = "",
        Action onChanged = null)
    {
        if (EqualityComparer<T>.Default.Equals(backingStore, value))
            return false;

        backingStore = value;
        onChanged?.Invoke();
        OnPropertyChanged(propertyName);
        return true;
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
    {
        var changed = PropertyChanged;
        if (changed == null)
            return;

        changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

public class StatusToTickedString: IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (bool)value ? "Ticked!" : "";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}