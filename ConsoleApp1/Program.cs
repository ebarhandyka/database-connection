using System.Data;
using System.Data.SqlClient;
using System.Numerics;
using System.Xml.Linq;

namespace DatabaseConnection;

class Program
{
    static string ConnectionString = "Data Source=DESKTOP-N6TO1LN;initial Catalog=db_hr_dts;Integrated Security=True;Connect Timeout=30";

    static SqlConnection connection;
    
    static void Main(string[] args)
    {

        /*try {
            connection.Open();
            Console.WriteLine("Koneksi Berhasil Dibuka!");
            connection.Close();
        } catch (Exception e) {
            Console.WriteLine(e.Message);
        }*/

        //Perintah menampilkan semua region pada tabel regions
        GetAllRegion();

        //Perintah menampilkan nama region berdasarkan region id yang diinputkan
        GetRegion(1);

        //Perintah menambahkan region berdasarkan nama region yang diinputkan
        InsertRegion("Australia");

        //Perintah mengupdate nama region berdasarkan region id yang diinputkan
        UpdateRegion(6, "Africa");

        //Perintah menghapus region pada tabel regions
        DeleteRegion(5);
    }

    // GETALL : REGION (Command)
    public static void GetAllRegion()
    {
        connection = new SqlConnection(ConnectionString);

        //Membuat instance untuk command
        SqlCommand command = new SqlCommand();
        command.Connection = connection;
        command.CommandText = "SELECT * FROM regions";

        //Membuka koneksi
        connection.Open();

        using SqlDataReader reader = command.ExecuteReader();
        if (reader.HasRows)
        {
            while (reader.Read())
            {
                Console.WriteLine("ID\t: " + reader[0]);
                Console.WriteLine("Name\t: " + reader[1]);
                Console.WriteLine("====================");
            }
        }
        else
        {
            Console.WriteLine("Data not found!");
        }
        reader.Close();
        connection.Close();
    }

    // GETBYID : REGION (Command)
    public static void GetRegion(int id)
    {
        connection = new SqlConnection(ConnectionString);

        //Membuat instance untuk command
        SqlCommand command = new SqlCommand();
        command.Connection = connection;
        command.CommandText = "SELECT region_name FROM regions WHERE region_id = @id";

        //Membuat parameter
        SqlParameter pID = new SqlParameter();
        pID.ParameterName = "@id";
        pID.Value = id;
        pID.SqlDbType = SqlDbType.VarChar;

        //Menambahkan parameter ke command
        command.Parameters.Add(pID);

        //Membuka koneksi
        connection.Open();

        using SqlDataReader reader = command.ExecuteReader();
        if (reader.HasRows)
        {
            while (reader.Read())
            {
                Console.WriteLine("Name\t: " + reader[0]);
                Console.WriteLine("====================");
            }
        }
        else
        {
            Console.WriteLine("Data not found!");
        }
        reader.Close();
        connection.Close();
    }


    // INSERT : REGION (Transaction)
    public static void InsertRegion(string name)
    {
        connection = new SqlConnection(ConnectionString);

        //Membuka koneksi
        connection.Open();

        SqlTransaction transaction = connection.BeginTransaction();
        try
        {
            //Membuat instance untuk command
            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandText = "INSERT INTO regions (region_name) VALUES (@name)";
            command.Transaction = transaction;

            //Membuat parameter
            SqlParameter pName = new SqlParameter();
            pName.ParameterName = "@name";
            pName.Value = name;
            pName.SqlDbType = SqlDbType.VarChar;

            //Menambahkan parameter ke command
            command.Parameters.Add(pName);

            //Menjalankan command
            int result = command.ExecuteNonQuery();
            transaction.Commit();

            if (result > 0)
            {
                Console.WriteLine("Data berhasil ditambahkan!");
            }
            else
            {
                Console.WriteLine("Data gagal ditambahkan!");
            }

            //Menutup koneksi
            connection.Close();

        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            try
            {
                transaction.Rollback();
            }
            catch (Exception rollback)
            {
                Console.WriteLine(rollback.Message);
            }
        }

    }

    // UPDATE : REGION (Transaction)
    public static void UpdateRegion(int id, string name)
    {
        connection = new SqlConnection(ConnectionString);

        //Membuka koneksi
        connection.Open();

        SqlTransaction transaction = connection.BeginTransaction();
        try
        {
            //Membuat instance untuk command
            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandText = "UPDATE regions SET region_name = @name WHERE region_id = @id";
            command.Transaction = transaction;

            //Membuat parameter
            SqlParameter pID = new SqlParameter();
            pID.ParameterName = "@id";
            pID.Value = id;
            pID.SqlDbType = SqlDbType.VarChar;

            SqlParameter pName = new SqlParameter();
            pName.ParameterName = "@name";
            pName.Value = name;
            pName.SqlDbType = SqlDbType.VarChar;

            //Menambahkan parameter ke command
            command.Parameters.Add(pID);
            command.Parameters.Add(pName);

            //Menjalankan command
            int result = command.ExecuteNonQuery();
            transaction.Commit();

            if (result > 0)
            {
                Console.WriteLine("Data berhasil diupdate!");
            }
            else
            {
                Console.WriteLine("Data gagal diupdate!");
            }

            //Menutup koneksi
            connection.Close();

        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            try
            {
                transaction.Rollback();
            }
            catch (Exception rollback)
            {
                Console.WriteLine(rollback.Message);
            }
        }

    }


    // DELETE : REGION (Transaction)
    public static void DeleteRegion(int id)
    {
        connection = new SqlConnection(ConnectionString);

        //Membuka koneksi
        connection.Open();

        SqlTransaction transaction = connection.BeginTransaction();
        try
        {
            //Membuat instance untuk command
            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandText = "DELETE FROM regions WHERE region_id=@id";
            command.Transaction = transaction;

            //Membuat parameter
            SqlParameter pID = new SqlParameter();
            pID.ParameterName = "@id";
            pID.Value = id;
            pID.SqlDbType = SqlDbType.VarChar;

            //Menambahkan parameter ke command
            command.Parameters.Add(pID);

            //Menjalankan command
            int result = command.ExecuteNonQuery();
            transaction.Commit();

            if (result > 0)
            {
                Console.WriteLine("Data berhasil dihapus!");
            }
            else
            {
                Console.WriteLine("Data gagal dihapus!");
            }

            //Menutup koneksi
            connection.Close();

        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            try
            {
                transaction.Rollback();
            }
            catch (Exception rollback)
            {
                Console.WriteLine(rollback.Message);
            }
        }

    }
}