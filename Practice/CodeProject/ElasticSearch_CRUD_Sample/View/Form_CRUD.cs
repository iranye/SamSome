using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Elastic_CRUD.View
{
    public partial class Form_CRUD : Form
    {
        private BLL.Customer _customerBll;

        /// <summary>
        /// Constructor
        /// </summary>
        public Form_CRUD()
        {
            InitializeComponent();
        }

        private void Form_CRUD_Load(object sender, EventArgs e)
        {
            try
            {
                _customerBll = new BLL.Customer();
                _customerBll.PropertyChanged += _customerBll_PropertyChanged;
                cboHasChildren.SelectedText = "false";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }

        private void _customerBll_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Status":
                    if (!String.IsNullOrWhiteSpace(_customerBll.Status))
                    {
                        printToOutputBox(_customerBll.Status);
                    }
                    break;
            }
        }

        /// <summary>
        /// Fill the ListView with Elastic return
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private void FillListView(List<DTO.Customer> list, ListView component)
        {
            BuildListView(component);

            for (int i = 0; i < list.Count; i++)
            {
                ListViewItem lvwItem = new ListViewItem();
                lvwItem.Text = list[i].Id.ToString();
                lvwItem.SubItems.Add(list[i].Name);
                lvwItem.SubItems.Add(list[i].Age.ToString());
                lvwItem.SubItems.Add(list[i].HasChildren.ToString());
                lvwItem.SubItems.Add(list[i].EnrollmentFee.ToString("C"));
                lvwItem.SubItems.Add(list[i].Opinion);
                component.Items.Add(lvwItem);
            }
        }

        private void BuildListView(ListView component)
        {
            component.Columns.Clear();
            component.Items.Clear();

            component.View = System.Windows.Forms.View.Details;
            component.FullRowSelect = true;
            component.GridLines = true;

            component.Columns.Add("Id", 50);
            component.Columns.Add("Name", 100);
            component.Columns.Add("Age", 50);
            component.Columns.Add("Has Children", 100);
            component.Columns.Add("Enrollment Fee", 100);
            component.Columns.Add("Opinion", 100);
        }
        
        private void BuildAggregationListView(ListView component)
        {
            component.Columns.Clear();
            component.Items.Clear();

            component.View = System.Windows.Forms.View.Details;
            component.FullRowSelect = true;
            component.GridLines = true;

            component.Columns.Add("Bucket", 150);
            component.Columns.Add("Value", 100);
        }
        
        private void FillAggregationListView(Dictionary<string, double> list, ListView component)
        {
            BuildAggregationListView(component);

            foreach (var item in list.Keys)
            {
                ListViewItem lvwItem = new ListViewItem();
                lvwItem.Text = item;
                lvwItem.SubItems.Add(list[item].ToString());
                component.Items.Add(lvwItem); 
            }
        }

        private DTO.Customer CreateEntityForSearching()
        {
            //TODO: Validation
            DTO.Customer entity = new DTO.Customer();            

            entity.Age = int.Parse(nudQryAge.Value.ToString());
            entity.Birthday = dtpQryBirthday.Value.ToString(DTO.Constants.BASIC_DATE);                        
            entity.EnrollmentFee = double.Parse(tbQryEnrFee.Text);
            entity.HasChildren = bool.Parse(cboQryChildren.Text);
            entity.Name = txtQryName.Text;
            entity.Id = int.Parse(tbQryId.Text);
            return entity;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            ClearOutputBox();
            bool inputValid = true;
            try
            {
                DTO.Customer entity = new DTO.Customer();

                entity.Id = int.Parse(tbId.Text);
                entity.Age = int.Parse(nudAge.Value.ToString());
                entity.Birthday = dtpBirthday.Value.ToString(DTO.Constants.BASIC_DATE);
                entity.EnrollmentFee = double.Parse(tbEnrollmentFee.Text);
                entity.HasChildren = bool.Parse(cboHasChildren.Text);
                if (String.IsNullOrWhiteSpace(txtName.Text))
                {
                    printToOutputBox("Name missing");
                    inputValid = false;
                }
                if (String.IsNullOrWhiteSpace(txtOpnion.Text))
                {
                    printToOutputBox("Opinion missing");
                    inputValid = false;
                }
                if (!inputValid)
                {
                    return;
                }
                entity.Name = txtName.Text;
                entity.Opinion = txtOpnion.Text;

                _customerBll.Index(entity);
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                printToOutputBox("ERROR: " + ex.Message);
            }
        }
        
        private void btnDeleting_Click(object sender, EventArgs e)
        {
            ClearOutputBox();
            int id;
            tbId.Text = tbId.Text.Trim();
            if (!Int32.TryParse(tbId.Text, out id))
            {
                printToOutputBox($"Invalid id: '{tbId.Text}'");
                return;
            }
            try
            {
                _customerBll.Delete(tbId.Text);
            }
            catch (Exception ex)
            {
                printToOutputBox(ex.Message);
            }
        }

        private void btnQrt1_Click(object sender, EventArgs e)
        {
            ClearOutputBox();
            try
            {
                FillListView(_customerBll.QueryById(tbQryId.Text), lvwHits);
            }
            catch (Exception ex)
            {
                printToOutputBox(ex.GetType() + Environment.NewLine + ex.Message);
            }
        }

        private void btnQrt2_Click(object sender, EventArgs e)
        {
            ClearOutputBox();
            try
            {
                DTO.Customer entity = CreateEntityForSearching();
                FillListView(_customerBll.QueryByAllFieldsUsingAnd(entity), lvwHits);
            }
            catch (Exception ex)
            {
                printToOutputBox(ex.GetType() + Environment.NewLine + ex.Message);
            }
        }

        private void btnQrt3_Click(object sender, EventArgs e)
        {
            ClearOutputBox();
            try
            {
                DTO.Customer entity = CreateEntityForSearching();
                FillListView(_customerBll.QueryByAllFieldsUsingOr(entity), lvwHits);
            }
            catch (Exception ex)
            {
                printToOutputBox(ex.GetType() + Environment.NewLine + ex.Message);
            }
        }
        
        private void btnQryComb1_Click(object sender, EventArgs e)
        {
            ClearOutputBox();
            try
            {
                DTO.CombinedFilter filter = new DTO.CombinedFilter();
                filter.HasChildren = bool.Parse(cboQryMustHasChildren.Text);
                filter.Ages = txtQryShouldHaveAges.Text.Split(',').ToList<string>();
                filter.Names = txtQryMustNotName.Text.Split(',').ToList<string>();

                FillListView(_customerBll.QueryUsingCombinations(filter), lvwHits2);
            }
            catch (Exception ex)
            {
                printToOutputBox(ex.GetType() + Environment.NewLine + ex.Message);
            }
        }

        private void btnQryRange1_Click(object sender, EventArgs e)
        {
            try
            {
                DTO.RangeFilter range = new DTO.RangeFilter();
                range.Birthday = dtpQryRangeDate.Value;
                range.EnrollmentFeeStart = double.Parse(mskQryRangeEnrol1.Text);
                range.EnrollmentFeeEnd = double.Parse(mskQryRangeEnrol2.Text);

                FillListView(_customerBll.QueryUsingRanges(range), lvwHits3);
            }
            catch (Exception ex)
            {
                printToOutputBox(ex.GetType() + Environment.NewLine + ex.Message);
            }
        }

        private void btnRunAgrr_Click(object sender, EventArgs e)
        {
            ClearOutputBox();
            try
            {
                //TODO: validation  (eg: Sum is not applicable to 'name' field)
                DTO.Aggregations aggFilter = new DTO.Aggregations();
                aggFilter.Field = cboQryField.Text;
                aggFilter.AggregationType = cboQryAggrType.Text;

                FillAggregationListView( _customerBll.GetAggregations(aggFilter), lvwHits4);

            }
            catch (Exception ex)
            {
                printToOutputBox(ex.GetType() + Environment.NewLine + ex.Message);
            }
        }

        private void printToOutputBox(string message)
        {
            tbOutput.Text += message += Environment.NewLine;
        }

        private void btnClearOut_Click(object sender, EventArgs e)
        {
            ClearOutputBox();
        }

        private void ClearOutputBox()
        {
            tbOutput.Text = String.Empty;
        }

        private void btCopyToClipboard_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(tbOutput.Text);
        }

        private void btnBulkIndex_Click(object sender, EventArgs e)
        {
            ClearOutputBox();
            _customerBll.BulkIndex();

        }
    }
}
