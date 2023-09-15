using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Alfonsovegacelis.ui
{
    public partial class frmCentroCosto : Form
    {
        bean.CentroCosto centroCosto = new bean.CentroCosto();
        dao.CentroCostoDAO centroCostoDAO = new dao.CentroCostoDAO();
        util.Util util = new util.Util();
        DataTable dtRegistros;
        bool bHayRegistros;
        int indexRegistro;


        public frmCentroCosto()
        {
            InitializeComponent();
        }

        private void frmCentroCosto_Load(object sender, EventArgs e)
        {
            getRegistros();
            Navegar(true);
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            Navegar(false);
            verRegistros(-1);
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {

        }
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            int id = txtID.Text == "" ? 0 : int.Parse(txtID.Text);
            string detelle = txtDetalle.Text;
            if (centroCostoDAO.Guardar(new bean.CentroCosto(id, detelle)))
            {
                getRegistros();
                txtBuscar_TextChanged(null, null);
                for (int i = 0; i < dgvRegistros.Rows.Count; i++)
                    if (dgvRegistros.Rows[i].Cells["Detalle"].Value.ToString().Equals(detelle))
                    { dgvRegistros.Rows[i].Cells["Detalle"].Selected = true; break; }
                MessageBox.Show("Registros " + (id == 0 ? "Guardado" : "Actualizado"));
            }

            Navegar(true);
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            Navegar(false);
        }
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Navegar(true);
            verRegistros(indexRegistro);
        }
        private void dgvRegistros_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            verRegistros(indexRegistro = e.RowIndex);
        }
        private void getRegistros()
        {
            dtRegistros = centroCostoDAO.getCentroCostos();
            if (bHayRegistros = dtRegistros != null)
            {
                dgvRegistros.DataSource = dtRegistros;
                dgvRegistros.Columns["id"].Visible = false;
                dgvRegistros.Columns["Detalle"].Width = dgvRegistros.Width - 3;
            }

        }
        private void verRegistros(int index)
        {

            bool bLimpiar = index == -1;
            txtID.Text = bLimpiar ? "" : dgvRegistros.Rows[index].Cells["id"].Value.ToString();
            txtDetalle.Text = bLimpiar ? "" : dgvRegistros.Rows[index].Cells["Detalle"].Value.ToString(); 

        }
        private void txt_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = util.LetterSpaceBack(sender, e);
        }

        private void txtDetalle_Leave(object sender, EventArgs e)
        {
            txtDetalle.Text = txtDetalle.Text.Trim();
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            dtRegistros.DefaultView.RowFilter = string.Format("Detalle like '%{0}%'", txtBuscar.Text);
        }
        private void Navegar(bool Bonof)
        {
            btnNuevo.Visible = Bonof;
            btnEditar.Visible = btnEliminar.Visible = Bonof && bHayRegistros;
            btnGuardar.Visible = btnCancelar.Visible = !Bonof;

            dgvRegistros.Enabled = Bonof && bHayRegistros;
            txtBuscar.Visible = Bonof && bHayRegistros;
            txtDetalle.ReadOnly = Bonof;

            if (!Bonof) txtDetalle.Focus();

        }
    }
}
