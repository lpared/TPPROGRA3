using System;
using MySql.Data.MySqlClient; 

namespace Progra3Card.Administrativo
{
    class Program
    {
        private static string connectionString = "Server=localhost;Database=mi_banco_db;Uid=root;Pwd=;";

        static void Main(string[] args)
        {
            bool salir = false;
            while (!salir)
            {
                Console.Clear();
                Console.WriteLine("========================================");
                Console.WriteLine("    SISTEMA ADMINISTRATIVO PROGRA3CARD   ");
                Console.WriteLine("========================================");
                Console.WriteLine("1. Emitir Nueva Tarjeta (Alta de Cliente)");
                Console.WriteLine("2. Listar Tarjetas");
                Console.WriteLine("3. Ver Detalle de una Tarjeta / Cliente");
                Console.WriteLine("4. Eliminar Tarjeta (Baja de Sistema)");
                Console.WriteLine("5. Emitir Nueva Liquidación Mensual");
                Console.WriteLine("6. Salir");
                Console.WriteLine("========================================");
                Console.Write("Seleccione una opción: ");

                switch (Console.ReadLine())
                {
                    case "1": MenuEmitirTarjeta(); break;
                    case "2": MenuListarTarjetas(); break;
                    case "3": MenuVerDetalleTarjeta(); break;
                    case "4": MenuEliminarTarjeta(); break;
                    case "5": MenuEmitirLiquidacion(); break;
                    case "6": salir = true; break;
                    default:
                        Console.WriteLine("Opción no válida. Presione una tecla para continuar...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        // Funciones a completar:

        static void MenuListarTarjetas()
        {
            Console.Clear();
            Console.WriteLine("--- LISTADO GENERAL DE TARJETAS ---");
            Console.WriteLine("{0,-12} {1,-18} {2,-20} {3,-15}", "Nro Cuenta", "Nro Tarjeta", "Banco Emisor", "DNI Titular");
            Console.WriteLine("----------------------------------------------------------------------");

            // === A realizar ===
            // Aquí deben implementar un SELECT sobre la tabla 'tarjetas'
            // para recorrer las filas e imprimirlas en la consola.
            
            ObtenerYMostrarTarjetas();

            Console.WriteLine("\nPresione una tecla para volver al menú...");
            Console.ReadKey();
        }

        static void MenuVerDetalleTarjeta()
        {
            Console.Clear();
            Console.WriteLine("--- DETALLE DE TARJETA Y CLIENTE ---");
            Console.Write("Ingrese el Número de Cuenta a consultar: ");
            int numCuenta = Convert.ToInt32(Console.ReadLine());

            // === A realizar ===
            // Aquí deben realizar un SELECT con un JOIN entre 'tarjetas' y 'usuarios' 
            // filtrando por el numCuenta para traer todos los campos (Nombre, Apellido, Email, Saldo, etc.)
            
            MostrarDetalleCompleto(numCuenta);

            Console.WriteLine("\nPresione una tecla para volver al menú...");
            Console.ReadKey();
        }

        static void MenuEliminarTarjeta()
        {
            Console.Clear();
            Console.WriteLine("--- ELIMINAR TARJETA DEL SISTEMA ---");
            Console.Write("Ingrese el Número de Cuenta de la tarjeta a dar de baja: ");
            int numCuenta = Convert.ToInt32(Console.ReadLine());

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n⚠️ ADVERTENCIA: Se eliminará la tarjeta, sus liquidaciones y los datos de acceso web vinculados.");
            Console.ResetColor();
            Console.Write("¿Está seguro de continuar? (S/N): ");
            
            if (Console.ReadLine().ToUpper() == "S")
            {
                // === A realizar ===
                // Aquí deben ejecutar un DELETE sobre la tabla 'tarjetas' donde num_cuenta = numCuenta.
                // Como definimos ON DELETE CASCADE en la base de datos, las liquidaciones se borrarán solas.
                // Opcional: Evaluar si también eliminan al usuario de la tabla 'usuarios' o si lo mantienen.
                
                bool exito = DarDeBajaTarjeta(numCuenta);

                if (exito)
                    Console.WriteLine("\nTarjeta eliminada correctamente del sistema.");
                else
                    Console.WriteLine("\nError al intentar eliminar la tarjeta. Verifique el número de cuenta.");
            }
            else
            {
                Console.WriteLine("\nOperación cancelada.");
            }

            Console.WriteLine("\nPresione una tecla para volver al menú...");
            Console.ReadKey();
        }


        // =========================================================================
        // MÉTODOS BASE QUE DEBEN COMPLETAR CON LA LÓGICA 
        // =========================================================================

        static void ObtenerYMostrarTarjetas()
        {
          using (MySqlConnection conn = new MySqlConnection(connectionString))
    {
        conn.Open();

        string query = "SELECT * FROM tarjetas";

        MySqlCommand cmd = new MySqlCommand(query, conn);

        MySqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            Console.WriteLine(
                "{0,-12} {1,-18} {2,-20} {3,-15}",
                reader["num_cuenta"],
                reader["numero_tarjeta"],
                reader["banco_emisor"],
                reader["dni_titular"]
            );
        }

        reader.Close();
    }
            // Completar 
            // Ejemplo de impresión dentro del bucle: 
            // Console.WriteLine("{0,-12} {1,-18} {2,-20} {3,-15}", reader["num_cuenta"], reader["numero_tarjeta"], ...);
        }

        static void MostrarDetalleCompleto(int cuenta)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = @"
                SELECT *
                FROM tarjetas t
                JOIN usuarios u
                    ON t.dni_titular = u.documento
                WHERE t.num_cuenta = @cuenta";
                MySqlCommand cmd= 
                new MySqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@cuenta", cuenta);
                MySqlDataReader reader = cmd.ExecuteReader();

                if(reader.Read())
                {
                    Console.WriteLine("====== DETALLE ======");
                    Console.WriteLine($"Cuenta: {reader["num_cuenta"]}");
                    Console.WriteLine($"Número de Tarjeta: {reader["numero_tarjeta"]}");
                    Console.WriteLine($"Banco Emisor: {reader["banco_emisor"]}");
                    Console.WriteLine($"Estado: {reader["estado"]}");
                    Console.WriteLine($"Saldo: {reader["saldo"]}");

                    Console.WriteLine();
                    Console.WriteLine("====== TITULAR ======");
                    Console.WriteLine($"Nombre:{reader["nombre"]} {reader["apellido"]}");
                    Console.WriteLine($"DNI: {reader["documento"]}");
                    Console.WriteLine($"Email: {reader["email"]}");
                    

                }
                else
                {
                    Console.WriteLine("No existe una tarjeta con ese numero de cuenta.");
                }
                reader.Close();
            }
            // Completar haciendo un SELECT con JOIN de usuarios y tarjetas WHERE num_cuenta = @cuenta
        }

        static bool DarDeBajaTarjeta(int cuenta)
        {
            using(MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "DELETE FROM tarjetas WHERE num_cuenta = @cuenta";

                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@cuenta", cuenta);

                int filasAfectadas= cmd.ExecuteNonQuery();

                return filasAfectadas > 0;
            }
            // Completar usando un DELETE FROM tarjetas WHERE num_cuenta = @cuenta
            return false;
        }
        static void MenuEmitirTarjeta()
{
    Console.Clear();
    Console.WriteLine("--- EMITIR NUEVA TARJETA ---");

    Console.Write("Documento: ");
    string documento = Console.ReadLine();

    Console.Write("Tipo de documento (DNI/PASAPORTE): ");
    string tipoDoc = Console.ReadLine().ToUpper();

    Console.Write("Nombre: ");
    string nombre = Console.ReadLine();

    Console.Write("Apellido: ");
    string apellido = Console.ReadLine();

    Console.Write("Fecha de nacimiento (AAAA-MM-DD): ");
    DateTime fechaNacimiento =
        Convert.ToDateTime(Console.ReadLine());

    Console.Write("Email: ");
    string email = Console.ReadLine();

    Console.WriteLine();
    Console.WriteLine("Seleccione Banco Emisor:");
    Console.WriteLine("1. Banco Nación");
    Console.WriteLine("2. Banco Provincia");
    Console.WriteLine("3. Banco Galicia");
    Console.WriteLine("4. Banco Santander");
    Console.WriteLine("5. Banco BBVA");
    Console.WriteLine("6. Banco Macro");

    Console.Write("Opción: ");
    int opcionBanco =
        Convert.ToInt32(Console.ReadLine());

    string banco = ObtenerBanco(opcionBanco);

    bool exito = CrearClienteYTarjeta(documento, tipoDoc, nombre, apellido, fechaNacimiento, email, banco
    );

    if (exito)
        Console.WriteLine("\nTarjeta emitida correctamente.");
    else
        Console.WriteLine("\nNo se pudo emitir la tarjeta.");

    Console.ReadKey();
}

static string ObtenerBanco(int opcion)
{
    switch (opcion)
    {
        case 1: return "Banco Nación";
        case 2: return "Banco Provincia";
        case 3: return "Banco Galicia";
        case 4: return "Banco Santander";
        case 5: return "Banco BBVA";
        case 6: return "Banco Macro";
        default: return "";
    }
}

static bool CrearClienteYTarjeta(
    string documento,
    string tipoDoc,
    string nombre,
    string apellido,
    DateTime fechaNacimiento,
    string email,
    string banco)
{
    using (MySqlConnection conn =
        new MySqlConnection(connectionString))
    {
        conn.Open();

        string sqlUsuario = @"
            INSERT INTO usuarios
            (documento,
             tipo_doc,
             nombre,
             apellido,
             fecha_nacimiento,
             email,
             usuario,
             password)
            VALUES
            (@doc,
             @tipo,
             @nombre,
             @apellido,
             @fecha,
             @email,
             NULL,
             NULL)";

        MySqlCommand cmdUsuario =
            new MySqlCommand(sqlUsuario, conn);

        cmdUsuario.Parameters.AddWithValue("@doc", documento);
        cmdUsuario.Parameters.AddWithValue("@tipo", tipoDoc);
        cmdUsuario.Parameters.AddWithValue("@nombre", nombre);
        cmdUsuario.Parameters.AddWithValue("@apellido", apellido);
        cmdUsuario.Parameters.AddWithValue("@fecha", fechaNacimiento);
        cmdUsuario.Parameters.AddWithValue("@email", email);

        cmdUsuario.ExecuteNonQuery();

        string numeroTarjeta =
            GenerarNumeroTarjeta();

        string sqlTarjeta = @"
            INSERT INTO tarjetas
            (numero_tarjeta,
             banco_emisor,
             estado,
             saldo,
             dni_titular)
            VALUES
            (@numero,
             @banco,
             'Activa',
             0,
             @dni)";

        MySqlCommand cmdTarjeta =
            new MySqlCommand(sqlTarjeta, conn);

        cmdTarjeta.Parameters.AddWithValue("@numero",numeroTarjeta);

        cmdTarjeta.Parameters.AddWithValue("@banco",banco);

        cmdTarjeta.Parameters.AddWithValue("@dni",documento);

        cmdTarjeta.ExecuteNonQuery();

        return true;
    }
}

static string GenerarNumeroTarjeta()
{
    Random rnd = new Random();

    string numero = "4512";

    for (int i = 0; i < 12; i++)
    {
        numero += rnd.Next(0, 10);
    }

    return numero;
}

static void MenuEmitirLiquidacion()
{
    Console.Clear();
    Console.WriteLine("--- EMITIR NUEVA LIQUIDACIÓN ---");

    Console.Write("Número de cuenta: ");
    int cuenta =
        Convert.ToInt32(Console.ReadLine());

    Console.Write("Período (AAAA-MM): ");
    string periodo =
        Console.ReadLine();

    Console.Write("Fecha de vencimiento (AAAA-MM-DD): ");
    DateTime vencimiento =
        Convert.ToDateTime(Console.ReadLine());

    Console.Write("Total a pagar: ");
    decimal total =
        Convert.ToDecimal(Console.ReadLine());

    Console.Write("Pago mínimo: ");
    decimal minimo =
        Convert.ToDecimal(Console.ReadLine());

    bool exito = CrearLiquidacion(
        cuenta,
        periodo,
        vencimiento,
        total,
        minimo);

    if (exito)
        Console.WriteLine("\nLiquidación emitida correctamente.");
    else
        Console.WriteLine("\nNo se pudo emitir la liquidación.");

    Console.ReadKey();
}

static bool CrearLiquidacion(
    int cuenta,
    string periodo,
    DateTime vencimiento,
    decimal total,
    decimal minimo)
{
    using (MySqlConnection conn =
        new MySqlConnection(connectionString))
    {
        conn.Open();

        string query = @"
            INSERT INTO liquidaciones
            (num_cuenta,
             periodo,
             fecha_vencimiento,
             total_a_pagar,
             pago_minimo)
            VALUES
            (@cuenta,
             @periodo,
             @vencimiento,
             @total,
             @minimo)";

        MySqlCommand cmd =
            new MySqlCommand(query, conn);

        cmd.Parameters.AddWithValue("@cuenta", cuenta);
        cmd.Parameters.AddWithValue("@periodo", periodo);
        cmd.Parameters.AddWithValue("@vencimiento", vencimiento);
        cmd.Parameters.AddWithValue("@total", total);
        cmd.Parameters.AddWithValue("@minimo", minimo);

        int filas =
            cmd.ExecuteNonQuery();

        return filas > 0;
    }
}}}