using BusinessLib;
using System.Windows.Controls;
using System.Windows.Input;

namespace TreeViewWithViewModelDemo.TextSearch
{
    public partial class TextSearchDemoControl : UserControl
    {
        readonly FamilyTreeViewModel _familyTree;

        public TextSearchDemoControl()
        {
            InitializeComponent();
            Person rootPerson = Database.GetFamilyTree();
            _familyTree = new FamilyTreeViewModel(rootPerson);
            DataContext = _familyTree;
            _familyTree.SearchText = "y";
        }

        private void searchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                _familyTree.SearchCommand.Execute(null);
        }
    }
}
