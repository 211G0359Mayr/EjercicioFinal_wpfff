using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.IO;
using Newtonsoft.Json;
using GalaSoft.MvvmLight.Command;
using EjercicioFinal.Models;

namespace EjercicioFinal.ViewModels
{
    public class PinacotecaViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        /// ////////////////////////////////////////////////////////////////////////////////////////////// <summary>
        //   Disculpe maestra Ernestina, la tardanza de este código es debido a que se me descompuso el regulador de la computadora desde el sábado,
        //   y no he estado usando la computadora más de 2 horas debido por el temor de que se me calentara demás o afectara al cargador ya que no
        //   haya nada de lo que regule, pido solamente un poco más de tiempo es probable que lo tenga terminado para la noche, debido a que me faltan
        //   unos ligeros detalles, se supone que lo debía de haber terminado desde el día de ayer pero el tiempo no me alcanzo, espero y comprenda mi
        //   situación, y me dé la oportunidad de entregarlo un poco más tarde, además de que estoy trabajando por las tardes así que mis tiempos libres
        //   para avanzar son pocos.
        /// </summary>
        public string Error { get; set; } = "";
        int indice;
        public ObservableCollection<Pinacoteca> Datos { get; set; } = new ObservableCollection<Pinacoteca>();
        public Pinacoteca Pinacoteca { get; set; } = new Pinacoteca();
        public string Vista { get; set; } = "ver";
        public ICommand? AgregarCommand { get; set; }
        public ICommand? EditarCommand { get; set; }
        public ICommand? EliminarCommand { get; set; }
        public ICommand? CambiarVistaCommand { get; set; }
        public ICommand? CancelarCommand { get; set; }


        public void Agregar()
        {
            Error = "";
            if (string.IsNullOrEmpty(Pinacoteca.Nombre))
            {
                Error += "Reingrese el nombre\n";
            }
            if (string.IsNullOrEmpty(Pinacoteca.Direccion))
            {
                Error += "Reingrese la direccion\n";
            }
            if (string.IsNullOrEmpty(Pinacoteca.Ciudad))
            {
                Error += "Reingrese la ciudad\n";
            }
            if (string.IsNullOrEmpty(Pinacoteca.MetrosCuadrados))
            {
                Error += "Reingrese la cantidad de metros cuadrados";
            }
            int i = 0;
            foreach (Pinacoteca item in Datos)
            {
                if (item.Nombre == Pinacoteca.Nombre)
                {
                    MessageBox.Show("Ingrese un nombre diferente");
                    return;
                }
                else
                {
                    i++;
                }
            }
            if (Error == "" && Datos != null)
            {
                Datos.Add(Pinacoteca);
                Guardar();
                CambiarVista("ver");
            }


        }

        public void CambiarVista(string vistaACambiar)
        {
            Vista = vistaACambiar;
            if (vistaACambiar == "agregar")
            {
                Pinacoteca = new Pinacoteca();
            }
            if (vistaACambiar == "editar")
            {
                if (Datos != null)
                {
                    indice = Datos.IndexOf(Pinacoteca);
                }
                Pinacoteca copiaPinacoteca = new Pinacoteca()
                {
                    Nombre = Pinacoteca.Nombre,
                    Direccion = Pinacoteca.Direccion,
                    Ciudad = Pinacoteca.Ciudad,
                    MetrosCuadrados = Pinacoteca.Ciudad
                };
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Vista"));
        }

        public void Cancelar()
        {
            CambiarVista("ver");
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Vista)));

        }

        public void Editar()
        {
            if (Datos != null)
            {
                Datos[indice] = Pinacoteca;
                Guardar();
                CambiarVista("ver");
            }
        }

        public void Eliminar()
        {
            if (Pinacoteca != null && Datos != null)
            {
                Datos.Remove(Pinacoteca);
                CambiarVista("ver");
                Guardar();
            }
        }

        public void Guardar()
        {
            var json = JsonConvert.SerializeObject(Datos);
            File.WriteAllText("Datos.json", json);
        }

        public void Cargar()
        {
            if (File.Exists("Datos.json"))
            {
                var json = File.ReadAllText("Datos.json");
                var datos = JsonConvert.DeserializeObject<ObservableCollection<Pinacoteca>>(json);
                if (datos != null)
                {
                    Datos = (ObservableCollection<Pinacoteca>)datos;
                }
                else
                {
                    Datos = new ObservableCollection<Pinacoteca>();
                }
            }
        }


        public PinacotecaViewModel()
        {
            Cargar();
            AgregarCommand = new RelayCommand(Agregar);
            EditarCommand = new RelayCommand(Editar);
            EliminarCommand = new RelayCommand(Eliminar);
            CancelarCommand = new RelayCommand(Cancelar);
            CambiarVistaCommand = new RelayCommand<string>(CambiarVista);
        }

    }
}
