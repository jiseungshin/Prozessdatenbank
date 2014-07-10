using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDCore.Processes;
using PDCore.BusinessObjects;

namespace OE110Prozessdatenbank.ViewModels
{
    public class ProcessQualityVM : BaseViewModel
    {
        BaseProcess m_process;
        public ProcessQualityVM(BaseProcess process)
        {
            m_process = process;
        }

        public bool GlassScratches
        {
            get { return m_process.Quality.GlassSratches; }
            set { m_process.Quality.GlassSratches = value; }
        }

        public bool GlassBreakage
        {
            get { return m_process.Quality.GlassBreakage; }
            set { m_process.Quality.GlassBreakage = value; }
        }

        public bool GlassPeeling
        {
            get { return m_process.Quality.GlassPeeling; }
            set { m_process.Quality.GlassPeeling = value; }
        }

        public int OverallResult
        {
            get { return m_process.Quality.OverallResult; }
            set { m_process.Quality.OverallResult = value; }
        }

        public int PV
        {
            get { return m_process.Quality.PV ?? 0; }
            set { m_process.Quality.PV = value; }
        }
    }

    public class WorkpieceQualityVM : BaseViewModel
    {
        Workpiece m_wp;
        public WorkpieceQualityVM(Workpiece wp)
        {
            m_wp = wp;
        }

        public string Reference
        { get { return m_wp.CurrentReferenceNumber.ToString(); } }

        public string Label
        { get { return m_wp.Label; } }

        public int Corrosion
        {
            get { return m_wp.Quality.Corrosion ?? -1; }
            set { m_wp.Quality.Corrosion = value; }
        }

        public int GlassAdherence
        {
            get { return m_wp.Quality.GlassAdherence ?? -1; }
            set { m_wp.Quality.GlassAdherence = value; }
        }

        public int MoldScratches
        {
            get { return m_wp.Quality.MoldScratches ??-1; }
            set { m_wp.Quality.MoldScratches = value; }
        }

        public int OverallResult
        {
            get { return m_wp.Quality.OverallResult ??-1; }
            set { m_wp.Quality.OverallResult = value; }
        }

        
    }
}
