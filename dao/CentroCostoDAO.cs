using Alfonsovegacelis.bean;
using System.Data;

namespace Alfonsovegacelis.dao
{
    internal class CentroCostoDAO
    {
        db.Db db = new db.Db("cnplanillas");
        public DataTable getCentroCostos()
        {
            db.Sentencia("sp_getCentroCostos");
            return db.getDataTable();
        }
        public bool Guardar(CentroCosto centroCosto)
        {
            db.Sentencia("sp_GuardarCentroCosto");
            db.Parametros(centroCosto.getColumnas());
            return db.Ejecutar() > 0;

        }
    }
}
