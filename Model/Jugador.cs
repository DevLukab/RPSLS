using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPTLS.Model
{
    public enum pptls { PEDRA, PAPER, TISORES, LLANGARDAIX, SPOCK };
    public class Jugador : IComparable<Jugador>
    {
        string id;
        string nom;
        int partidesGuanyadesTorneig;
        int puntuacio;
        int partidesGuanyades;
        int rondesGuanyades;
        int rondesPerdudes;

        public string Foto { get; set; }
        public string Id { get => this.id; set => this.id = value; }
        public string Nom { get => this.nom; set => this.nom = value; }
        public int PartidesGuanyadesTorneig { get => this.partidesGuanyadesTorneig; set => this.partidesGuanyadesTorneig = value; }
        public int Puntuacio { get => this.puntuacio; set => this.puntuacio = value; }
        public int PartidesGuanyades { get => this.partidesGuanyades; set => this.partidesGuanyades = value; }
        public int RondesGuanyades { get => this.rondesGuanyades; set => this.rondesGuanyades = value; }
        public int RondesPerdudes { get => this.rondesPerdudes; set => this.rondesPerdudes = value; }

        public int CompareTo(Jugador? other)
        {
            if (other == null)
            {
                return 1;
            }

            int compareResult = other.PartidesGuanyadesTorneig.CompareTo(this.PartidesGuanyadesTorneig);
            if (compareResult != 0)
            {
                return compareResult;
            }

            compareResult = this.Puntuacio.CompareTo(other.Puntuacio);
            if (compareResult != 0)
            {
                return compareResult;
            }

            compareResult = other.PartidesGuanyades.CompareTo(this.PartidesGuanyades);
            if (compareResult != 0)
            {
                return compareResult;
            }

            compareResult = other.RondesGuanyades.CompareTo(this.RondesGuanyades);
            if (compareResult != 0)
            {
                return compareResult;
            }

            compareResult = this.RondesPerdudes.CompareTo(other.RondesPerdudes);
            if (compareResult != 0)
            {
                return compareResult;
            }

            return this.Nom.CompareTo(other.Nom);
        }

        public override string ToString()
        {
            return Nom + "(" + Puntuacio + ")";
        }
    }
}
