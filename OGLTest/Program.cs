using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OGLTest
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var window = new MyWindow())
            {
                window.Run(60.0);
            }
        }
    }
}
