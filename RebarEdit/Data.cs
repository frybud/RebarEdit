using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;


namespace RebarEdit
{
    public class Data
    {
        private string m_strRebar;
        private int m_selCount;
        private Element m_elemlocal;
        private string m_tagName;
        private Parameter m_para;
        private Dictionary<string, string> m_beamdata = new Dictionary<string, string>();

        public Dictionary<string,string> beamdata
        {
            get
            {
                return m_beamdata;
            }
            set
            {
                m_beamdata = value;
            }
        }

        public Parameter para
        {
            get
            {
                return m_para;
            }
            set
            {
                m_para = value;
            }
        }
        public string tagName
        {
            get
            {
                return m_tagName;
            }
            set
            {
                m_tagName = value;
            }
        }
        public string strRebar
        {
            get
            {
                return m_strRebar;
            }
            set
            {
                m_strRebar = value;
            }
        }
        public int selCount
        {
            get
            {
                return m_selCount;
            }
            set
            {
                m_selCount = value;
            }
        }

        public void ObtainData(ExternalCommandData commandData)
        {

            if (null == commandData)
            {
                throw new ArgumentNullException("commandData");
            }


            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;
            Selection selection = uidoc.Selection;
            ICollection<ElementId> selectedIds = selection.GetElementIds();
            m_selCount = selectedIds.Count;
            ElementId id = selectedIds.Last();
            Element elem = doc.GetElement(id);
            IndependentTag indTag = elem as IndependentTag;
            m_strRebar = indTag.TagText;
            m_tagName = elem.Name;
            m_elemlocal = doc.GetElement(indTag.TaggedLocalElementId);
            m_para = m_elemlocal.LookupParameter(m_tagName);

            string infotemp = m_elemlocal.Name.ToString();
            string[] strArray = infotemp.Split(new char[] { 'X' });



            m_beamdata.Add("bb", strArray[0]);
            m_beamdata.Add("bh", strArray[1]);
            m_beamdata.Add("bnth", getpara("梁编号"));
            m_beamdata.Add("bgj", getpara("单梁箍筋"));
            m_beamdata.Add("bleft", getpara("单梁支座上部纵筋（左）"));
            m_beamdata.Add("btj", getpara("单梁上部通长筋或架立筋"));
            m_beamdata.Add("bright", getpara("单梁支座上部纵筋（右）"));
            m_beamdata.Add("bbj", getpara("单梁下部纵筋"));
            m_beamdata.Add("bnj", getpara("单梁构造筋或扭筋"));

        }
        private string getpara(String parastr)
        {
            string paratxt;
            try
            {
                paratxt = m_elemlocal.LookupParameter(parastr).AsString();

            }
            catch
            {
                paratxt = "0";
                
            }
            return paratxt;
        }

    }
}
