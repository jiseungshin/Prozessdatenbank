using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDCore.Processes;

namespace OE110Prozessdatenbank.ViewModels
{
    public class QualityVM : BaseViewModel
    {
        BaseProcess m_process;
        public QualityVM(BaseProcess process)
        {
            m_process = process;
        }

        public bool GlassScratches
        {
            get { return m_process.Quality.PQuality.GlassSratches; }
            set { m_process.Quality.PQuality.GlassSratches = value; }
        }
    }
}
