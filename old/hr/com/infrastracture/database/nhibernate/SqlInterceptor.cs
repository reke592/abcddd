using System.Net;
using System.Net.Sockets;
using System.Text;
using NHibernate;
using NHibernate.SqlCommand;

namespace hr.com.infrastracture.database.nhibernate {
    public class SqlInterceptor : EmptyInterceptor {
        Socket server;
        byte[] message = new byte[] { 1 };
        byte[] reset = Encoding.ASCII.GetBytes("reset");
        byte[] recv = new byte[256];
        bool connected = false;

        public static bool send_data = false;

        public SqlInterceptor(string serv_addr = "127.0.0.1", int port = 4000) {
            try {
                server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                server.Connect(IPAddress.Parse(serv_addr), port);
                server.Send(reset);
                this.connected = true;
            }
            catch {
                this.connected = false;
            }
        }

        ~SqlInterceptor() {
            if(connected)
                server.Disconnect(true);
        }

        public override SqlString OnPrepareStatement(NHibernate.SqlCommand.SqlString sql) {
            if(connected && send_data) {
                server.Send(Encoding.ASCII.GetBytes(sql.ToString()));
                while(server.Receive(recv, 0, server.Available, SocketFlags.None) == 0);
            }
            return sql;
        }
    }
}