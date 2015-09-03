using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;

namespace ifunction.MicrosoftSqlDeployTool
{
    /// <summary>
    /// Class for advanced form
    /// </summary>
    public partial class AdvancedForm : Form
    {
        /// <summary>
        /// The configuration XML
        /// </summary>
        protected XElement configurationXml = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdvancedForm"/> class.
        /// </summary>
        /// <param name="includedXml">The included XML.</param>
        /// <param name="configurationXml">The configuration XML.</param>
        public AdvancedForm(XElement configurationXml)
        {
            InitializeComponent();
            this.configurationXml = configurationXml;

            this.tree_Scripts.CheckBoxes = true;
            this.tree_Scripts.ShowLines = true;
            this.tree_Scripts.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawAll;
            this.tree_Scripts.DrawNode += new System.Windows.Forms.DrawTreeNodeEventHandler(this.treeView_DrawNode);
        }

        /// <summary>
        /// Handles the DrawNode event of the treeView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DrawTreeNodeEventArgs"/> instance containing the event data.</param>
        private void treeView_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            if (e.Node.Level == 1)
            {
                Color backColor, foreColor;

                backColor = e.Node.TreeView.BackColor;
                foreColor = e.Node.TreeView.ForeColor;

                using (SolidBrush brush = new SolidBrush(backColor))
                {
                    e.Graphics.FillRectangle(brush, e.Node.Bounds);
                }

                TextRenderer.DrawText(e.Graphics, e.Node.Text, this.tree_Scripts.Font, e.Node.Bounds, foreColor, backColor);

                if ((e.State & TreeNodeStates.Focused) == TreeNodeStates.Focused)
                {
                    ControlPaint.DrawFocusRectangle(e.Graphics, e.Node.Bounds, foreColor, backColor);
                }

                e.DrawDefault = false;
            }
            else
            {
                e.DrawDefault = true;
            }
        }

        /// <summary>
        /// Handles the Load event of the AdvancedForm control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void AdvancedForm_Load(object sender, EventArgs e)
        {
            RenderTree(this.tree_Scripts, this.configurationXml);
        }

        /// <summary>
        /// Renders the tree.
        /// </summary>
        /// <param name="tree">The tree.</param>
        /// <param name="configurationXml">The x element.</param>
        private void RenderTree(TreeView tree, XElement configurationXml)
        {
            tree.BeginUpdate();
            tree.Nodes.Clear();

            if (configurationXml != null)
            {
                foreach (var one in configurationXml.Elements())
                {
                    var addedNode = tree.Nodes.Add(one.Name.LocalName);
                    bool enabled = false;
                    if (bool.TryParse(one.GetAttributeValue(Constants.Enabled), out enabled) && enabled)
                    {
                        addedNode.Checked = true;
                    }

                    foreach (var item in one.Elements(Constants.Item))
                    {
                        if (!string.IsNullOrWhiteSpace(item.Value))
                        {
                            var itemAdded = addedNode.Nodes.Add(item.Value);
                        }
                    }
                }
            }
            tree.EndUpdate();
        }

        /// <summary>
        /// Handles the Click event of the btn_Save control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void btn_Save_Click(object sender, EventArgs e)
        {
            foreach (TreeNode item in this.tree_Scripts.Nodes)
            {
                XElement xNode = (from one in this.configurationXml.Elements() where one.Name.LocalName == item.Text select one).FirstOrDefault();
                if (xNode != null)
                {
                    xNode.SetAttributeValue(Constants.Enabled, item.Checked);
                }
            }

            this.Close();
        }

        /// <summary>
        /// Handles the Click event of the btn_Cancel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
