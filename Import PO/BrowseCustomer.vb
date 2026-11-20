Imports System.IO
Imports AppUtility.UI.Notification
Public Class BrowseCustomer
    Private _selectedCustomer As ARCustomerDTO
    Private _customers As List(Of ARCustomerDTO)
    Private Sub BrowseCustomer_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        GridControl1.ForceInitialize()
        GridView1.FocusedRowHandle = DevExpress.XtraGrid.GridControl.AutoFilterRowHandle
        GridView1.FocusedColumn = GridView1.Columns("Name")
        GridView1.ShowEditor()
        FillData()
    End Sub
    Private Sub FillData()
        File.Delete("C:\Temp\customer.xml")
        Dim objPost As New PurchaseOrderBuild
        _customers = objPost.getCustomer()



        ARCustomerDTOBindingSource.DataSource = _customers
        GridView1.RefreshData()
    End Sub
    Public ReadOnly Property SelectedCustomer As ARCustomerDTO
        Get
            Return _selectedCustomer
        End Get
    End Property

    Private Sub ArCustomerBindingSource_CurrentChanged(sender As Object, e As EventArgs)


    End Sub
    Private Sub AcceptCustomer()
        If _selectedCustomer IsNot Nothing Then

            Close()
        End If
    End Sub

    Private Sub BarButtonItem1_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem1.ItemClick
        AcceptCustomer()
    End Sub

    Private Sub BarButtonItem2_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem2.ItemClick
        _selectedCustomer = Nothing
        Close()
    End Sub

    Private Sub GridControl1_DoubleClick(sender As Object, e As EventArgs) Handles GridControl1.DoubleClick
        AcceptCustomer()
    End Sub

    Private Sub BarButtonItem1_ItemPress(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem1.ItemPress
        AcceptCustomer()
    End Sub

    Private Sub ARCustomerDTOBindingSource_CurrentChanged(sender As Object, e As EventArgs) Handles ARCustomerDTOBindingSource.CurrentChanged
        _selectedCustomer = DirectCast(ARCustomerDTOBindingSource.Current, ARCustomerDTO)
    End Sub
End Class