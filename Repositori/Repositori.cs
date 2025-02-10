using PPTLS.Repositori.XML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPTLS.Repositori
{
    public static class Repo
    {
        private static IRepositoriDeJugadors? repositoriDeJugadors = null;

        public static IRepositoriDeJugadors ObreBDJugador()
        {
            if (repositoriDeJugadors == null)
            {
                repositoriDeJugadors = new JugadorsXML();
            }
            return repositoriDeJugadors;
        }
    }
}
