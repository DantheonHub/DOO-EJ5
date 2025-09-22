 namespace DOO_EJ5
{
    internal class Program
    {
        static void Main(string[] args)
        {
            BaseDatos bdOrigen = new BaseDatos(
                "http://localhost:3306",
                "DB_Principal",
                new string[] { "registro1", "registro2", "registro3" }
            );

            ComputadoraVirtual vm = new ComputadoraVirtual(
                "VM1",
                "1.0",
                "Windows 10",
                Estado.Down,
                bdOrigen
            );

            ProcesarDatos proceso = new ProcesarDatos(vm);

            Aplicacion app = new Aplicacion(vm, "C#", "12.0", null);

            bool salir = false;
            while (!salir)
            {
                Console.WriteLine("\n--- Menú ---");
                Console.WriteLine("1. Levantar VM");
                Console.WriteLine("2. Bajar VM");
                Console.WriteLine("3. Clonar BD");
                Console.WriteLine("4. Filtrar BD");
                Console.WriteLine("5. Almacenar BD");
                Console.WriteLine("6. Levantar proceso");
                Console.WriteLine("7. Bajar proceso");
                Console.WriteLine("8. Levantar aplicación");
                Console.WriteLine("9. Bajar aplicación");
                Console.WriteLine("0. Salir");
                Console.Write("Elija una opción: ");

                string opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1":
                        vm.Levantar();
                        break;
                    case "2":
                        vm.Bajar();
                        break;
                    case "3":
                        proceso.ClonarBD();
                        app.bd = proceso.DatosFin;
                        break;
                    case "4":
                        proceso.FiltrarBD();
                        break;
                    case "5":
                        proceso.AlmacenarBD();
                        break;
                    case "6":
                        proceso.Levantar();
                        break;
                    case "7":
                        proceso.Bajar();
                        break;
                    case "8":
                        app.Levantar();
                        break;
                    case "9":
                        app.Bajar();
                        break;
                    case "0":
                        salir = true;
                        break;
                    default:
                        Console.WriteLine("Opción inválida");
                        break;
                }
            }

            Console.WriteLine("Programa finalizado.");
        }
    }
 
    public enum Estado
    {
        Down, Up 
    }
    public interface IAccion
    {
         void Levantar();
         void Bajar();
    }
    public class BaseDatos
    {
        public string Url;
        public string nombre;
        public string[] datos;

        public BaseDatos(string url, string nombre, string[] datos)
        {
            this.Url = url;
            this.nombre = nombre;
            this.datos = datos;
        }
    }

    public class ComputadoraVirtual : IAccion
    {
        public string nombre;
        public string version;
        public string sistemaOperativo;
        public Estado estado;
        public BaseDatos mibaseDatos;
      public ComputadoraVirtual() { }
      public ComputadoraVirtual(string nombre, string version, string sistemaOperativo, Estado estado, BaseDatos bd = null)
        {
            this.nombre = nombre;
            this.version = version;
            this.sistemaOperativo = sistemaOperativo;
            this.estado = estado;
            this.mibaseDatos = bd;
        }
        public void Levantar()
        {
            if (this.estado == Estado.Up)
            {
                Console.WriteLine($"La computadora {nombre} ya está levantada.");
                return;
            }
            this.estado = Estado.Up;
            Console.WriteLine($"La computadora {nombre} se levantó correctamente.");
        }
        public void Bajar()
        {
            if (estado == Estado.Down)
            {
                Console.WriteLine($"La computadora {this.nombre} ya está apagada.");
                return;
            }
            estado = Estado.Down;
            Console.WriteLine($"La computadora {this.nombre} se bajó correctamente.");
        }
    }
    public class ProcesarDatos : IAccion
    {
        public ComputadoraVirtual DatosOrigen;
        public BaseDatos DatosFin;

        public ProcesarDatos() { }
        public ProcesarDatos(ComputadoraVirtual Origen)
        {
            this.DatosOrigen = Origen;
        }
        public void ClonarBD() 
        {
            if (DatosOrigen.estado != Estado.Up)
            {
                Console.WriteLine("No se puede clonar. La máquina de origen está apagada.");
                return;
            }

            if (DatosOrigen.mibaseDatos == null || DatosOrigen.mibaseDatos.datos.Length == 0)
            {
                Console.WriteLine("No hay datos de origen disponibles para clonar.");
                return;
            }

            Console.Clear();
            Console.WriteLine($"Clonando Base de datos desde {DatosOrigen.mibaseDatos.Url}");
            DatosFin = new BaseDatos(DatosOrigen.mibaseDatos.Url, DatosOrigen.mibaseDatos.nombre + "_copia", DatosOrigen.mibaseDatos.datos );

            Console.WriteLine($"Clonación exitosa! \nUrl: {DatosFin.Url} \nNombre: {DatosFin.nombre}\nDatos: {string.Join(",", DatosFin.datos)}");
        }
        public void FiltrarBD()
        {
            if(DatosFin == null || DatosFin.datos.Length == 0)
            {
                Console.WriteLine("No hay bases de datos para clonar");
                return;
            }
            Console.WriteLine($"Filtrando datos en {this.DatosFin.nombre}..");
        }

        public void AlmacenarBD()
        {
            if (DatosFin == null || DatosFin.datos.Length == 0)
            {
                    Console.WriteLine("No hay base de datos para almacenar");
                    return;
            }
            Console.WriteLine($"Base de datos {DatosFin.nombre} almacenada con éxito. Contiene {DatosFin.datos.Length} registros.");
        }
        public void Levantar()
        {
            if (DatosOrigen.estado != Estado.Up)
            {
                Console.WriteLine("La máquina está apagada. Levantando ahora...");
                DatosOrigen.Levantar();
            }

            if (DatosOrigen.mibaseDatos != null && DatosFin != null)
            {
                Console.WriteLine("La instancia de ProcesarDatos se levantó correctamente.");
                Console.WriteLine($"Acceso a datos de origen: {DatosOrigen.mibaseDatos.nombre} almacenados.");
                Console.WriteLine($"Acceso a datos de fin: {DatosFin.nombre} almacenados.");
            }
            else
            {
                Console.WriteLine("No se puede levantar la instancia. Verifique la base de datos de origen y destino.");
            }
        }
        public void Bajar()
        {
            DatosOrigen.Bajar();
            Console.WriteLine("La instancia de ProcesarDatos se bajó correctamente.");
        }
    }


    public class Aplicacion : IAccion
    {
        public string lenguaje;
        public string versionLenguaje;
        public BaseDatos bd;
        public ComputadoraVirtual maquina;
        //public string versionDeseada;
        public Aplicacion(ComputadoraVirtual maquina, string lenguaje, string version, BaseDatos bd)
        {
            this.maquina = maquina;
            this.lenguaje = lenguaje;
            this.versionLenguaje = version;
            this.bd = bd;
        }

        public void Levantar()
        {
            if (maquina.estado != Estado.Up)
            {
                Console.WriteLine("La máquina está apagada. Levantando ahora...");
                maquina.Levantar();
            }

            if (maquina != null && bd != null)
            {
                Console.WriteLine("La instancia de aplicación se levantó correctamente.");
                Console.WriteLine($"Lenguaje instalado: {lenguaje} (Versión: {versionLenguaje})");
                Console.WriteLine($"Acceso a la base de datos: {bd.nombre} ({bd.Url})");
            }
            else
            {
                Console.WriteLine("No se puede levantar la aplicacion. Verifique la máquina y la base de datos.");
            }
        }
        public void Bajar()
        {
            if (maquina != null)
            {
                maquina.estado = Estado.Down;
                Console.WriteLine("La instancia de aplicación se bajo correctamente.");
            }
            else { Console.WriteLine("No se puede levantar la aplicacion. Verifique la maquina y la base de datos."); }
        }
    }
}
/*
Las de aplicación son instancias que nos permitirán ser la base para las aplicaciones que los desarrolladores deseen implementar o publicar en nuestra nube 
por lo que debe almacenarse:
lenguaje de programación:
Versión del lenguaje:
Base de datos: (url o ubicación de la base de datos).
Al momento de “levantar” la instancia debe confirmar que se ha instalado el lenguaje de programación en la versión deseada y
que se posee acceso correcto a la base de datos, por lo que al levantarse debería cambiar de estado a 1 o “up” y mostrar en pantalla que se 
ha levantado correctamente la instancia, que se instaló el lenguaje (lenguaje de programación almacenado) en la versión (versión almacenada) 
y que posee acceso a la base de datos (base de datos almacenada”.

Se necesita instanciar al menos dos maquinar virtuales de cada tipo dentro de un arreglo de maquinas virtuales para poder levantarlas y bajarlas en simultaneo, 
y que cada clase realice el proceso almacenado independientemente.
 */