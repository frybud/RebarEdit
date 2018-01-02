using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using System.Text.RegularExpressions;
using System.Windows.Forms;


namespace RebarEdit
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]
    public class Command : IExternalCommand
    {

        public Autodesk.Revit.UI.Result Execute(ExternalCommandData commandData, ref string message, ElementSet element)
        {

            Transaction tran = new Transaction(commandData.Application.ActiveUIDocument.Document, "Edit Rebar");
            tran.Start();

            try
            {
                if (null == commandData)
                {
                    throw new ArgumentNullException("commandData");
                }
                UIDocument uidoc = commandData.Application.ActiveUIDocument;


                if (tagname(uidoc) == "梁编号")
                {
                    TaskDialog.Show("Revit", "不能选择梁编号标记!");

                }
                else
                {

                    Data data = new Data();
                    data.ObtainData(commandData);
                    if (data.selCount != 1)
                    {
                        TaskDialog.Show("Revit", "没选中或选中了多个构件!");
                    }
                    else
                    {
                        EditForm frm = new EditForm(data);
                        frm.StartPosition = FormStartPosition.CenterScreen;


                        if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            addmodified(uidoc);
                            data.para.Set(frm.res);
                            tran.Commit();
                            return Autodesk.Revit.UI.Result.Succeeded;
                        }
                        else
                        {
                            tran.RollBack();
                            return Autodesk.Revit.UI.Result.Cancelled;
                        }

                    }
                }
                tran.Commit();
                return Autodesk.Revit.UI.Result.Succeeded;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                tran.RollBack();
                return Result.Failed;
            }



        }
        static public void addmodified(UIDocument uidoc)
        {
            Document doc = uidoc.Document;
            Selection selection = uidoc.Selection;
            ICollection<ElementId> selectedIds = selection.GetElementIds();
            ElementId id = selectedIds.Last();
            Element elem = doc.GetElement(id);
            IndependentTag indTag = elem as IndependentTag;

            Element elemlocal = doc.GetElement(indTag.TaggedLocalElementId);
            elemlocal.LookupParameter("标记").Set(DateTime.Now.ToString("yyyyMMddHHmmss"));

        }
        static public string tagname(UIDocument uidoc)
        {
            Document doc = uidoc.Document;
            Selection selection = uidoc.Selection;
            ICollection<ElementId> selectedIds = selection.GetElementIds();
            ElementId id = selectedIds.Last();
            Element elem = doc.GetElement(id);
            IndependentTag indTag = elem as IndependentTag;
            string tagName = elem.Name;
            return tagName;
        }

    }
}
