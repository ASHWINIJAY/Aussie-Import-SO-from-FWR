Imports System.Data.SqlClient
Imports System.IO
Imports System.Net
Imports System.Net.Mail
Imports System.Text
Imports System.Xml

Public Class frmImportPo2
    Private _validated As Boolean = False

    Private dblOrdQty, dblPrice, dblExRate As Double
    Private strStockcode, strRef, strSupplier, strBuyer As String
    Private strWarehouse As String = ""
    Private _comment As String = ""
    Private Sub SimplebtnButton1_Click(sender As Object, e As EventArgs) Handles SimplebtnButton1.Click
        If export = False Then
            AppUtility.UI.Notification.ErrorMsg("You cannot close the application until change the export status")
            Return
        End If
        Me.Close()
    End Sub

    Private Sub btnBrowse_Click(sender As Object, e As EventArgs)
        Dim myStream As Stream = Nothing
        Dim openFileDialog1 As New OpenFileDialog()

        'openFileDialog1.InitialDirectory = "c:\"
        openFileDialog1.Filter = "Excel (*.xls)|*.xls|Excel 2007(*.xlsx)|*.xlsx|All files (*.*)|*.*"
        openFileDialog1.FilterIndex = 2
        openFileDialog1.RestoreDirectory = True

        If openFileDialog1.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
            Try
                myStream = openFileDialog1.OpenFile()
                If (myStream IsNot Nothing) Then
                    _validated = False
                    ' txtPath.Text = openFileDialog1.FileName
                End If
            Catch Ex As Exception
                MessageBox.Show("Cannot read file from disk. Original error: " & Ex.Message)
            Finally
                ' Check this again, since we need to make sure we didn't throw an exception on open. 
                If (myStream IsNot Nothing) Then
                    myStream.Close()
                    btnPost.Enabled = False
                End If
            End Try
        End If
    End Sub

    Private Sub btnValidate_Click(sender As Object, e As EventArgs) Handles btnValidate.Click
        'If lblWarehouse.Text = "" Then
        '    MsgBox("Please select a warehouse")
        '    Return
        'End If
        'If lblWarehouse.Text = "40" Then
        '    Dim value = MsgBox(" You are not selecting any customer and warehouse, Are you sure want to go with default customer and warehouse? ", MsgBoxStyle.YesNo, "Warning ")
        '    If value.ToString = "No" Then
        '        Return
        '    End If
        'End If
        If Trim(txtUser.Text) <> "" Then
            If Trim(txtComp.Text) <> "" Then
                _sysUsername = Trim(txtUser.Text)
                _sysPass = Trim(txtPass.Text)
                _sysComp = Trim(txtComp.Text)
                _sysCompPass = Trim(txtCompPass.Text)
                AppUtility.UI.Notification.ShowWaitForm(Me, "Validation in progress...")
                If Testlogin() Then
                    'Select Case optType.SelectedIndex
                    '    Case 0

                    ValidateStockedFile()
                    '    Case 1
                    '        ValidateNonStockedFile()
                    '    Case Else
                    '        MsgBox("Please Select either stocked or non stocked Option. Transaction Cancelled.", vbExclamation)
                    'End Select
                    ' _validated = True
                    If _validated Then
                        btnPost.Enabled = True
                    End If

                End If
            Else
                MsgBox("Please enter the Sypro company", MsgBoxStyle.Exclamation)
                txtComp.Focus()
            End If
            AppUtility.UI.Notification.CloseWaitForm()
        Else
            MsgBox("Please enter your Syspro usrname", MsgBoxStyle.Exclamation)
            txtUser.Focus()
        End If
    End Sub
    Dim failedvalidOrders As New List(Of String)
    Dim failedExportPO As New List(Of String)
    Dim failedvalidOrdersReason As New List(Of String)
    Private Function ValidateStockedFile() As Boolean
        _validated = False
        Dim strFilePath As String = "" ' Trim(txtPath.Text)
        ' Insert code to read the stream here. (load dataset )
        Dim ds As DataSet = Nothing
        Dim dr As SOHdr = Nothing
        Dim dtl As SODtl = Nothing
        Dim dt As DataTable = Nothing
        Dim intCount As Integer = 1
        'Dim dblOrdQty, dblPrice, dblExRate As Double
        Dim strStockcode, strSupplier As String
        Dim objSyspro As New SysproObj
        Dim objPo As New PurchaseOrderMasterHdr
        Dim objPost As New PurchaseOrderBuild
        Dim obj As New frmValidate
        Dim objQry As New Query
        Dim sb As New StringBuilder

        Dim strFirstWarehouse As String = Nothing
        Dim intFirstExRate As Double = Nothing
        Dim strFirstBuyer As String = Nothing
        Dim strFirstSupplier As String = Nothing
        Dim strFirstRef As String = Nothing
        Dim intErrCount As Integer = 0

        failedvalidOrders.Clear()
        failedvalidOrdersReason.Clear()
        failedExportPO.Clear()
        If APISalesOrders.Count > 0 Then
            Dim isError = False
            Try

                Dim intRCount As Integer = APISalesOrders.Count

                obj.Show()

                sb.AppendLine("Errors found in FWR Customer File  import:")
                sb.AppendLine("=======================================")
                sb.AppendLine()

                For Each dr In APISalesOrders

                    Dim sb1 As New StringBuilder
                    'Validate each line in the sheet as returned by dataset

                    'If objPost.CheckDuplicateOrder(dr.PO) Then
                    '    obj.ListBox1.Items.Add(" This Purchase order number " & dr.PO & " .")
                    '    obj.ListBox1.Items.Add(" Has already be used For customer 1509 FWR cannot import.")
                    '    sb.AppendLine(" This Purchase order number" & dr.PO & " . Has already be used for customer 1509 FWR cannot import.")
                    '    intErrCount = intErrCount + 1
                    '    obj.lblErr.Text = intErrCount
                    'End If
                    'If Convert.ToDateTime(dr.ShipDate) < DateTime.Now.Date Then
                    '    obj.ListBox1.Items.Add("Ship Date should be future date for this order " & dr.PO)
                    '    sb.AppendLine("Ship Date should be future date for this order " & dr.PO)
                    '    sb1.AppendLine("Ship Date should be future date for this order " & dr.PO)
                    '    intErrCount = intErrCount + 1
                    '    obj.lblErr.Text = intErrCount
                    '    isError = True
                    'End If
                    'dr.CustomerName = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa"
                    If dr.CustomerName.Length > 50 Then
                        obj.ListBox1.Items.Add("Warning! Maximum of 50 charctor are allowed for the customer name on the order " & dr.PO)
                        dr.CustomerName = dr.CustomerName.Substring(0, 49)
                    End If
                    If dr.ShipAddr1.Length > 40 Then
                        obj.ListBox1.Items.Add("Warning! Ship Address 1 is more than 40 charactor  on the order " & dr.PO)
                        dr.ShipAddr1 = dr.ShipAddr1.Substring(0, 39)
                    End If
                    If dr.ShipAddr2.Length > 40 Then
                        obj.ListBox1.Items.Add("Warning! Ship Address 2 is more than 40 charactor  on the order " & dr.PO)
                        dr.ShipAddr2 = dr.ShipAddr2.Substring(0, 39)
                    End If
                    If dr.ShipAddr3.Length > 40 Then
                        obj.ListBox1.Items.Add("Warning! Ship Address 3 is more than 40 charactor  on the order " & dr.PO)
                        dr.ShipAddr3 = dr.ShipAddr3.Substring(0, 39)
                    End If
                    For Each dtl In dr.SODtl
                        If dtl.ItemCode Is DBNull.Value Then
                            obj.ListBox1.Items.Add("Stockcode is empty on the order " & dr.PO)
                            sb.AppendLine("Stockcode is empty on the order " & dr.PO)
                            sb1.AppendLine("Stockcode is empty on the order " & dr.PO)
                            intErrCount = intErrCount + 1
                            obj.lblErr.Text = intErrCount
                            isError = True
                        Else
                            File.Delete("C:\Temp\Stock.xml")
                            Dim uom = objPost.getUOM(dtl.ItemCode)
                            If uom = "Failed" Then
                                obj.ListBox1.Items.Add("Stockcode " & dtl.ItemCode & " is not found in syspro on the order " & dr.PO)
                                sb.AppendLine("Stockcode " & dtl.ItemCode & " is not found in syspro on the order " & dr.PO)
                                intErrCount = intErrCount + 1
                                obj.lblErr.Text = intErrCount
                                sb1.AppendLine("Stockcode " & dtl.ItemCode & " is not found in syspro on the order " & dr.PO)

                                isError = True
                            Else
                                dtl.UOM = uom
                            End If
                            Dim ware = objPost.getWarehouse(dtl.ItemCode)
                            If ware = False Then
                                obj.ListBox1.Items.Add("Stockcode " & dtl.ItemCode & " is not mapped with warehouse 40 in syspro on the order " & dr.PO)
                                sb.AppendLine("Stockcode " & dtl.ItemCode & " is not mapped with warehouse 40 in syspro on the order " & dr.PO)
                                intErrCount = intErrCount + 1
                                obj.lblErr.Text = intErrCount
                                sb1.AppendLine("Stockcode " & dtl.ItemCode & " is not mapped with warehouse 40 in syspro on the order " & dr.PO)
                                isError = True
                            End If
                            Dim ware1 = objPost.getStock(dtl.ItemCode)
                            If ware1 = False Then
                                obj.ListBox1.Items.Add("Stockcode " & dtl.ItemCode & " is on hold in syspro on the order " & dr.PO)
                                sb.AppendLine("Stockcode " & dtl.ItemCode & " is on hold in syspro on the order " & dr.PO)
                                intErrCount = intErrCount + 1
                                obj.lblErr.Text = intErrCount
                                sb1.AppendLine("Stockcode " & dtl.ItemCode & " is on hold in syspro on the order " & dr.PO)
                                isError = True
                            End If
                            Dim unitprice = objPost.getUnitPrice(dtl.ItemCode)
                            If unitprice = -0.1 Then
                                obj.ListBox1.Items.Add("Unitprice is not found for this stockcode " & dtl.ItemCode & " . Order Number: " & dr.PO)
                                sb.AppendLine("Unitprice is not found for this stockcode " & dtl.ItemCode & " . Order Number: " & dr.PO)
                                intErrCount = intErrCount + 1
                                obj.lblErr.Text = intErrCount
                                sb1.AppendLine("Unitprice is not found for this stockcode " & dtl.ItemCode & " . Order Number: " & dr.PO)
                                isError = True
                            Else
                                dtl.UnitPrice = unitprice
                            End If
                        End If

                        If dtl.Qty <= 0 Then
                            obj.ListBox1.Items.Add("Order qty should be greater than 0 for this stockcode " & dtl.ItemCode & " .order " & dr.PO)
                            sb.AppendLine("Order qty should be greater than 0 for this stockcode " & dtl.ItemCode & " . order " & dr.PO)
                            intErrCount = intErrCount + 1
                            obj.lblErr.Text = intErrCount
                            sb1.AppendLine("Order qty should be greater than 0 for this stockcode " & dtl.ItemCode & " . order " & dr.PO)
                            isError = True
                        End If
                        ' strBuyer = dr("Buyer").ToString

                    Next

                    'If txtShipDate.EditValue Is Nothing Then


                    '    If dr("Deliv# date") Is DBNull.Value Then
                    '        obj.ListBox1.Items.Add("Delivery date is empty on line " & intCount + 1)
                    '        sb.AppendLine("Delivery date is empty on line " & intCount + 1)
                    '        intErrCount = intErrCount + 1
                    '        obj.lblErr.Text = intErrCount

                    '    Else
                    '        If Convert.ToDateTime(dr("Deliv# date")) < DateTime.Now.Date Then
                    '            obj.ListBox1.Items.Add("Delivery Date should be future date on line " & intCount + 1)
                    '            sb.AppendLine("Delivery Date should be future date on line " & intCount + 1)
                    '            intErrCount = intErrCount + 1
                    '            obj.lblErr.Text = intErrCount
                    '        End If
                    '    End If

                    'End If




                    'If dr("Net price") Is DBNull.Value Then
                    '    obj.ListBox1.Items.Add("Net price is empty on line " & intCount + 1)
                    '    sb.AppendLine("Net price is empty on line " & intCount + 1)
                    '    intErrCount = intErrCount + 1
                    '    obj.lblErr.Text = intErrCount
                    'Else
                    '    If Convert.ToDecimal(dr("Net price")) <= 0 Then
                    '        obj.ListBox1.Items.Add("Net price should be greater than 0 on line " & intCount + 1)
                    '        sb.AppendLine("Net price should be greater than 0 on line " & intCount + 1)
                    '        intErrCount = intErrCount + 1
                    '        obj.lblErr.Text = intErrCount
                    '    End If
                    'End If


                    'If dr("Warehouse") Is DBNull.Value Then
                    '    obj.ListBox1.Items.Add("Warehouse is empty on line " & intCount + 1)
                    '    sb.AppendLine("Warehouse is empty on line " & intCount + 1)
                    '    intErrCount = intErrCount + 1
                    '    obj.lblErr.Text = intErrCount
                    'End If
                    If isError Then
                        failedvalidOrders.Add(dr.PO)
                        failedvalidOrdersReason.Add(sb1.ToString())
                    End If
                    Using outfile As New StreamWriter("C:\Temp\ValidateErrs.txt")
                        outfile.Write(sb.ToString())
                    End Using
                    intCount += 1
                Next
                'If txtShipDate.EditValue IsNot Nothing Then



                '    If Convert.ToDateTime(txtShipDate.EditValue) < DateTime.Now.Date Then
                '        obj.ListBox1.Items.Add("Delivery Date should be future date ")

                '    End If


                'End If
            Catch ex As Exception
                MsgBox(ex.Message & " Please check file and try again.")
                Return _validated
            End Try
            AppUtility.UI.Notification.CloseWaitForm()
            If isError = False Then
                obj.ListBox1.Items.Add(vbNewLine)
                obj.ListBox1.Items.Add("No Errors found in file")

                obj.ListBox1.Items.Add(vbNewLine)
                obj.ListBox1.Items.Add(APISalesOrders.Count.ToString() + " Salesorder(s) will be created")
                '  btnPost.Enabled = True
                _validated = True
            Else
                If AppUtility.UI.Notification.ConfirmMsg("Validation failed for " & failedvalidOrders.Count & " PO(s). Do you want to continue for other orders?") Then
                    _validated = True
                    APISalesOrders = APISalesOrders.Where(Function(c) Not failedvalidOrders.Contains(c.PO)).ToList()
                End If
            End If

        Else
            MsgBox("No Salesorder found")

        End If

        Return _validated

    End Function
    Dim results As New List(Of String)

    Private Sub btnPost_Click(sender As Object, e As EventArgs) Handles btnPost.Click
        export = False
        SOResults.Clear()
        Dim strFilePath As String = "" ' Trim(txtPath.Text)
        ' Insert code to read the stream here. (load dataset )
        Dim ds As DataSet = Nothing
        Dim dr As DataRow = Nothing
        Dim dt As DataTable = Nothing
        Dim intCount As Integer = 1
        Dim objSyspro As New SysproObj
        Dim objPo As New PurchaseOrderMasterHdr
        Dim objSo As New SOHdr
        Dim objPost As New PurchaseOrderBuild
        Dim obj As New frmValidate
        Dim _stocked As Boolean = Nothing

        If _validated Then
            AppUtility.UI.Notification.ShowWaitForm(Me, "Sales Order Creation In Progress..")
            If APISalesOrders.Count > 0 Then
                File.Delete("C:\Temp\SOR.xml")
                Dim sb As New StringBuilder


                Dim intRCount As Integer = APISalesOrders.Count
                Dim i = 0
                Dim result = "Salesorder(s) Created" & vbNewLine
                sb.AppendLine(result)
                obj.ListBox1.Items.Add("Salesorder(s) Created Successfully" & vbNewLine)
                For i = 0 To APISalesOrders.Count - 1
                    objSo = APISalesOrders(i)


                    File.Delete("C:\Temp\SOR.xml")
                    Dim so = objPost.post_SOOrder(objSo)
                    result += so & vbNewLine
                    obj.ListBox1.Items.Add(so & vbNewLine)
                    sb.AppendLine(so & vbNewLine)
                Next
                If APISalesOrders.Count = SOResults.Count Then
                    btnChangeStatus.Enabled = True
                Else
                    btnChangeStatus.Enabled = True
                    AppUtility.UI.Notification.CloseWaitForm()
                    'AppUtility.UI.Notification.InformationMsg("Error occured when creating salesorder, We cannot enable change order status button")
                End If
                obj.Show()
                Using outfile As New StreamWriter("C:\Temp\ValidateErrs.txt")
                    outfile.Write(sb.ToString())
                End Using
                ' MsgBox(result)
                'build PO details 
                _arCustomer = Nothing

                ' lblCustomer.Text = ""
                _inWarehouse = Nothing
                'lblWarehouse.Text = ""

                btnPost.Enabled = False
                btnValidate.Enabled = False
            Else
                MsgBox("No PO found", , "Import SO")
                Exit Sub
            End If
            AppUtility.UI.Notification.CloseWaitForm()
            saveLog()
        Else
            MsgBox("PO(s) not validated. Please validate file first.", MsgBoxStyle.Exclamation)
        End If

    End Sub
    Private Sub saveLog()
        Try


            Dim connString = System.Configuration.ConfigurationManager.ConnectionStrings("AussieConn").ConnectionString
            If _sysComp = "Z" Then
                connString = connString.Replace("AussieTravellerCompanyN", "AussieTravellerCompanyZ")
            End If

            Dim con As New SqlConnection(connString)
            Dim salesOrderList As String = String.Join(", ", SOResults)
            Dim POList As String = String.Join(", ", APIOriginalSalesOrders.Select(Function(c) c.PO))
            Dim SuccessOrders As String = String.Join(", ", APISalesOrders.Select(Function(c) c.PO))
            Dim failedReason As String = String.Join(", ", failedvalidOrdersReason)
            failedReason += " " + String.Join(", ", DupOrdersReason)
            Dim failedOrders As String = String.Join(", ", failedvalidOrders)
            failedOrders += " " + String.Join(", ", DupOrders)
            Dim filePath As String = "C:\Temp\APIResponse.xml" ' Replace with the actual path to your XML file
            Dim xml As XElement = XElement.Load(filePath)
            Dim tot = (failedvalidOrders.Count + DupOrders.Count)
            Dim qry = "Insert into ImportFWRAuditLogs(NoOfSuccess,NoOfFailure,Salesorders,CustomerPOs,Payload,CreatedUser,FailedReason,TotalOrders,FailedPOs,SuccessPOs,FailedUpdateExportPOs,FailedUpdateExportPOsReason) values(" & SOResults.Count & "," & tot & ",'" & salesOrderList & "','" & POList & "','" & xml.ToString().Replace("'", "") & "','" & _sysUsername & "','" & failedReason & "'," & APIOriginalSalesOrders.Count & ",'" & failedOrders & "','" & SuccessOrders & "','" & SuccessOrders & "','Forget to click export status');"
            Dim sqlCommand As New SqlCommand(qry, con)

            con.Open()

            sqlCommand.ExecuteNonQuery()
            qry = "select LogId from ImportFWRAuditLogs order by LogId desc"
            sqlCommand = New SqlCommand(qry, con)
            Dim SqlAda = New SqlDataAdapter(sqlCommand)
            Dim ds = New DataSet()
            SqlAda.Fill(ds)

            If ds.Tables(0).Rows.Count > 0 Then
                Auditid = ds.Tables(0).Rows(0).Item(0)
            End If
            con.Close()
        Catch ex As Exception

        End Try
    End Sub
    Private Sub UpdateMailLog()
        Try


            Dim connString = System.Configuration.ConfigurationManager.ConnectionStrings("AussieConn").ConnectionString
            If _sysComp = "Z" Then
                connString = connString.Replace("AussieTravellerCompanyN", "AussieTravellerCompanyZ")
            End If

            Dim con As New SqlConnection(connString)


            Dim qry = "Update ImportFWRAuditLogs set IsMailSent=1,FromMailAddr='" & fromMail & "',ToMailAddr='" & toMail & "' where LogId=" & Auditid
            Dim sqlCommand As New SqlCommand(qry, con)

            con.Open()
            'Auditid = sqlCommand.ExecuteScalar()
            sqlCommand.ExecuteNonQuery()
            con.Close()
        Catch ex As Exception

        End Try
    End Sub
    Private Sub UpdateExportLog()
        Try


            Dim connString = System.Configuration.ConfigurationManager.ConnectionStrings("AussieConn").ConnectionString
            If _sysComp = "Z" Then
                connString = connString.Replace("AussieTravellerCompanyN", "AussieTravellerCompanyZ")
            End If

            Dim con As New SqlConnection(connString)
            Dim failedexport As String = ""

            failedexport = String.Join(", ", failedExportPO)
            Dim qry = ""
            If failedExportPO.Count > 0 Then
                qry = "Update ImportFWRAuditLogs set IsExported=1,FailedUpdateExportPOs='" & failedexport & "',FailedUpdateExportPOsReason='Timeout/network Error'  where LogId=" & Auditid
            Else
                qry = "Update ImportFWRAuditLogs set IsExported=1,FailedUpdateExportPOs='',FailedUpdateExportPOsReason=''  where LogId=" & Auditid
            End If

            Dim sqlCommand As New SqlCommand(qry, con)

            con.Open()
            'Auditid = sqlCommand.ExecuteScalar()
            sqlCommand.ExecuteNonQuery()
            con.Close()
        Catch ex As Exception

        End Try
    End Sub
    Dim Auditid = 0
    Private Sub Build_StockLine(objSo As SOHdr, intcount As Integer, stockcode As String, qty As String, price As Decimal, shipdate As Date, uom As String, warehouse As String)
        ' objSo.SODtl.Add(New SODtl(intcount, stockcode, qty, price, shipdate, uom, warehouse))
    End Sub

    Private Function ValidateImportData(dr As DataRow, intCount As Integer, _stocked As Boolean) As Boolean
        Dim _valid As Boolean = False
        Try
            If dr("OrderQty") Is DBNull.Value Then
                MsgBox("OrderQty is empty on line " & intCount + 1 & ". Please check the file and re-import. Import terminated.")
                GoTo validationfail
            Else
                dblOrdQty = dr("OrderQty").ToString
            End If

            If dr("Price") Is DBNull.Value Then
                MsgBox("Price is empty on line " & intCount + 1 & ". Please check the file and re-import. Import terminated.")
                GoTo validationfail
            Else
                dblPrice = dr("Price").ToString
            End If

            If dr("Buyer") Is DBNull.Value Then
                MsgBox("Buyer is empty on line " & intCount + 1 & ". Please check the file and re-import. Import terminated.")
                GoTo validationfail
            Else
                strBuyer = dr("Buyer").ToString
            End If
            If dr("Ship Date") Is DBNull.Value Then
                MsgBox("ShipDate is empty on line " & intCount + 1 & ". Please check the file and re-import. Import terminated.")
                GoTo validationfail
            Else

            End If
            If dr("Supplier") Is DBNull.Value Then
                MsgBox("Supplier is empty on line " & intCount + 1 & ". Please check the file and re-import. Import terminated.")
                GoTo validationfail
            Else
                strSupplier = dr("Supplier").ToString
            End If

            If dr("Exchange Rate") Is DBNull.Value Then
                MsgBox("Exchange Rate is empty on line " & intCount + 1 & ". Please check the file and re-import. Import terminated.")
                GoTo validationfail
            Else
                dblExRate = dr("Exchange Rate").ToString
            End If

            If dr("OurRef") Is DBNull.Value Then
                MsgBox("Our Ref is empty on line " & intCount + 1 & ". Please check the file and re-import. Import terminated.")
                GoTo validationfail
            Else
                strRef = dr("OurRef").ToString
            End If

            If Not dr("Comment") Is DBNull.Value Then
                _comment = Trim((dr("Comment")).ToString)
            End If

            Select Case _stocked
                Case True
                    If (dr("StockCode")) Is DBNull.Value Then
                        MsgBox("StockCode is empty on line " & intCount + 1 & ". Please check the file and re-import. Import terminated.")
                        GoTo validationfail
                    Else
                        strStockcode = dr("StockCode").ToString
                    End If
                    If dr("Warehouse") Is DBNull.Value Then
                        MsgBox("Warehouse is empty on line " & intCount + 1 & ". Please check the file and re-import. Import terminated.")
                        GoTo validationfail
                    Else
                        strWarehouse = dr("Warehouse").ToString
                    End If

                Case False
                    If (dr("NonStockCode")) Is DBNull.Value Then
                        MsgBox("StockCode is empty on line " & intCount + 1 & ". Please check the file and re-import. Import terminated.")
                        GoTo validationfail
                    Else
                        strStockcode = dr("NonStockCode").ToString
                    End If

                    If dr("ProductClass") Is DBNull.Value Then
                        MsgBox("ProductClass is empty on line " & intCount + 1 & ". Please check the file and re-import. Import terminated.")
                        GoTo validationfail
                    End If

                    If dr("Description") Is DBNull.Value Then
                        MsgBox("Description is empty on line " & intCount + 1 & ". Please check the file and re-import. Import terminated.")
                        GoTo validationfail
                    End If

                    'If dr("GlCode") Is DBNull.Value Then
                    '    MsgBox("GlCode is empty on line " & intCount + 1 & ". Please check the file and re-import. Import terminated.")
                    'End If
            End Select
            _valid = True

validationfail:

        Catch ex As Exception
            MsgBox(ex.Message & " Please check file and try again.")
        End Try

        Return _valid
    End Function

    Private Sub frmImpoPo2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        initializeform()
        If _username IsNot Nothing Then
            Dim args = _username.ToString().Split(",")
            txtUser.Text = args(1)
            txtPass.Text = args(2)
        End If
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs)
        Dim saveFileDialog1 As New FolderBrowserDialog
        Dim dataDirectory As String = String.Format("{0}\", Environment.CurrentDirectory)
        Dim a = dataDirectory & "\SO Import Template.xlsx"
        If (saveFileDialog1.ShowDialog() = DialogResult.OK) Then


            Dim c = saveFileDialog1.SelectedPath & "\SO Import Template.xlsx"
            FileCopy(a, c)
            MsgBox("Import Template Successfully created")
        End If
    End Sub
    Public _arCustomer As ARCustomerDTO
    Public _inWarehouse As InvWhControlDTO
    Private Function getXML(update As Boolean, order As String)
        'Declaration
        Dim Document As New System.Text.StringBuilder

        'Building Document content
        With Document
            .Append("<?xml version=""1.0"" encoding=""utf-8""?>")
            .Append("<GetOrder>")
            .Append("<Filter>")
            .Append("<WarehouseID>13</WarehouseID>")
            .Append("<OrderComplete>Approved</OrderComplete>")
            .Append("<OrderStatus>Pick</OrderStatus>")
            .Append("<ExportStatus>Pending</ExportStatus>")
            If order <> "" Then
                .Append("<OrderID>" & order & "</OrderID>")
            End If
            .Append("<OutputSelector>ID</OutputSelector>")
            .Append("<OutputSelector>ShippingOption</OutputSelector>")
            .Append("<OutputSelector>DeliveryInstruction</OutputSelector>")
            .Append("<OutputSelector>Username</OutputSelector>")
            .Append("<OutputSelector>Email</OutputSelector>")
            .Append("<OutputSelector>ShipAddress</OutputSelector>")
            .Append("<OutputSelector>ShipCompany</OutputSelector>")
            .Append("<OutputSelector>BillAddress</OutputSelector>")
            .Append("<OutputSelector>CustomerRef1</OutputSelector>")
            .Append("<OutputSelector>CustomerRef2</OutputSelector>")
            .Append("<OutputSelector>CustomerRef3</OutputSelector>")
            .Append("<OutputSelector>CustomerRef4</OutputSelector>")
            .Append("<OutputSelector>SalesChannel</OutputSelector>")
            .Append("<OutputSelector>GrandTotal</OutputSelector>")
            .Append("<OutputSelector>ShippingTotal</OutputSelector>")
            .Append("<OutputSelector>ShippingDiscount</OutputSelector>")
            .Append("<OutputSelector>OrderType</OutputSelector>")
            .Append("<OutputSelector>OrderStatus</OutputSelector>")
            .Append("<OutputSelector>OrderPayment</OutputSelector>")
            .Append("<OutputSelector>OrderPayment.DatePaid</OutputSelector>")
            .Append("<OutputSelector>DatePlaced</OutputSelector>")
            .Append("<OutputSelector>DateRequired</OutputSelector>")
            .Append("<OutputSelector>DateInvoiced</OutputSelector>")
            .Append("<OutputSelector>DatePaid</OutputSelector>")
            .Append("<OutputSelector>OrderLine</OutputSelector>")
            .Append("<OutputSelector>OrderLine.ProductName</OutputSelector>")
            .Append("<OutputSelector>OrderLine.PickQuantity</OutputSelector>")
            .Append("<OutputSelector>OrderLine.BackorderQuantity</OutputSelector>")
            .Append("<OutputSelector>OrderLine.UnitPrice</OutputSelector>")
            .Append("<OutputSelector>OrderLine.WarehouseID</OutputSelector>")
            .Append("<OutputSelector>OrderLine.WarehouseName</OutputSelector>")
            .Append("<OutputSelector>OrderLine.WarehouseReference</OutputSelector>")
            .Append("<OutputSelector>OrderLine.Quantity</OutputSelector>")
            .Append("<OutputSelector>OrderLine.PercentDiscount</OutputSelector>")
            .Append("<OutputSelector>OrderLine.ProductDiscount</OutputSelector>")
            .Append("<OutputSelector>OrderLine.CostPrice</OutputSelector>")
            .Append("<OutputSelector>OrderLine.ShippingMethod</OutputSelector>")
            .Append("<OutputSelector>OrderLine.ShippingTracking</OutputSelector>")
            .Append("<OutputSelector>ShippingSignature</OutputSelector>")
            If update Then
                .Append("<UpdateResults>")
                .Append("<ExportStatus>Exported</ExportStatus>")
                .Append("</UpdateResults>")
            End If
            .Append("</Filter>")
            .Append("</GetOrder>")
            Return Document.ToString
        End With

    End Function
    Public Function APIResponse(ByVal xml_Path As String) As List(Of SOHdr)
        Dim listSOHdr = New List(Of SOHdr)
        Dim tr As XmlTextReader = New XmlTextReader(xml_Path)
        Dim doc As New XmlDocument
        Dim sbWH As New StringBuilder
        doc.Load(xml_Path)
        tr.Read()
        'Dim objHdr = New SOHdr()
        'objHdr.PO = "tes"
        'objHdr.OrderDate = DateTime.Now
        'objHdr.ShipDate = DateAdd(DateInterval.Day, 1, objHdr.OrderDate)
        'objHdr.CustomerCode = "WEI002"
        'objHdr.Warehouse = "FG"

        'Dim listOrderDtl As New List(Of SODtl)
        'Dim stockLine = 0
        '' For Each dtl As XmlElement In po.SelectNodes("OrderLine")
        'stockLine += 1
        'Dim objDtl = New SODtl(stockLine, "FG63595", 1, 100, objHdr.ShipDate, "EA", objHdr.Warehouse)
        'listOrderDtl.Add(objDtl)
        '' Next
        'objHdr.SODtl = listOrderDtl
        ' listSOHdr.Add(objHdr)
        Try


            For Each po As XmlElement In doc.SelectNodes("GetOrderResponse/Order")
                If po.InnerXml = "" Then
                    tr.Close()
                    Return listSOHdr
                End If
                Dim objHdr = New SOHdr()
                objHdr.PO = po.SelectSingleNode("ID").InnerText
                objHdr.OrderDate = po.SelectSingleNode("DatePlaced").InnerText
                objHdr.ShipDate = DateAdd(DateInterval.Day, 1, objHdr.OrderDate)
                objHdr.CustomerCode = "1509"
                objHdr.Warehouse = 40
                Dim cus = ""
                Dim ship = ""
                If po.SelectSingleNode("ShipCompany") IsNot Nothing Then
                    cus = po.SelectSingleNode("ShipCompany").InnerText.Replace("&", " ")
                End If
                If po.SelectSingleNode("ShipStreetLine1") IsNot Nothing Then
                    ship = po.SelectSingleNode("ShipStreetLine1").InnerText.Replace("&", " ")
                End If
                If po.SelectSingleNode("ShipStreetLine2") IsNot Nothing Then
                    ship = ship & " " & po.SelectSingleNode("ShipStreetLine2").InnerText.Replace("&", " ")
                End If
                objHdr.CustomerName = po.SelectSingleNode("ShipFirstName").InnerText & " " & po.SelectSingleNode("ShipLastName").InnerText & " " & cus
                objHdr.CustomerName = objHdr.CustomerName.ToUpper().Replace("&", " ")
                objHdr.ShipAddr1 = ship
                objHdr.ShipAddr2 = po.SelectSingleNode("ShipState").InnerText.Replace("&", " ")
                objHdr.ShipAddr3 = po.SelectSingleNode("ShipCity").InnerText.Replace("&", " ")
                objHdr.ShipZip = po.SelectSingleNode("ShipPostCode").InnerText.Replace("&", " ")
                objHdr.ShipPhone = po.SelectSingleNode("ShipPhone").InnerText.Replace("&", " ")

                Dim listOrderDtl As New List(Of SODtl)
                Dim stockLine = 0
                For Each dtl As XmlElement In po.SelectNodes("OrderLine")
                    stockLine += 1
                    Dim objDtl = New SODtl(stockLine, dtl.SelectSingleNode("SKU").InnerText, Convert.ToDecimal(dtl.SelectSingleNode("Quantity").InnerText), Convert.ToDecimal(dtl.SelectSingleNode("CostPrice").InnerText), objHdr.ShipDate, "EA", objHdr.Warehouse, Convert.ToDecimal(dtl.SelectSingleNode("PercentDiscount").InnerText))
                    listOrderDtl.Add(objDtl)
                Next
                objHdr.SODtl = listOrderDtl
                listSOHdr.Add(objHdr)
            Next
        Catch ex As Exception

        End Try
        tr.Close()
        Return listSOHdr
    End Function
    Dim APISalesOrders As New List(Of SOHdr)
    Dim APIOriginalSalesOrders As New List(Of SOHdr)
    Private Sub btnCallAPI_Click(sender As Object, e As EventArgs) Handles btnCallAPI.Click
        APISalesOrders.Clear()
        APIOriginalSalesOrders.Clear()
        SOResults.Clear()
        AppUtility.UI.Notification.ShowWaitForm(Me, "Fetching orders from API...")
        Dim xmlpath As String = getXML(False, "")
        Dim API = "https://www.caravanrvcamping.com.au/do/WS/NetoAPI"
        Dim request As HttpWebRequest = DirectCast(WebRequest.Create(API), HttpWebRequest)
        Dim bytes() As Byte
        bytes = System.Text.Encoding.ASCII.GetBytes(xmlpath)
        request.ContentType = "application/xml"
        request.ContentLength = bytes.Length
        request.Method = "POST"
        request.Accept = "*/*"

        ' request.Headers.Add("Accept", "*/*")
        'request.Headers.Add("Accept-Encoding", "application/gzip,deflate,br")
        ' request.Headers.Add("Connection", "keep-alive")
        request.Headers.Add("NETOAPI_ACTION", "GetOrder")
        request.Headers.Add("NETOAPI_USERNAME", "crcfeed")
        request.Headers.Add("NETOAPI_KEY", "nQzisw8oZ2hT5oGQDEgjFy1CuLXH172I")


        Dim requestStream As Stream = request.GetRequestStream()
        requestStream.Write(bytes, 0, bytes.Length)
        requestStream.Close()
        Dim response As HttpWebResponse
        response = DirectCast(request.GetResponse(), HttpWebResponse)
        Dim filePath = "C:\Temp\APIResponseAuto.xml"

        If File.Exists(filePath) Then

            File.Delete(filePath)
        End If
        If response.StatusCode = HttpStatusCode.OK Then
            Dim responseStream As Stream = response.GetResponseStream()
            Dim responseStr As String = New StreamReader(responseStream).ReadToEnd()
            Dim dirPath As String = Path.GetDirectoryName(filePath)

            If Not Directory.Exists(dirPath) Then
                Directory.CreateDirectory(dirPath)
            End If
            If SaveTextToFile(responseStr, "C:\Temp\APIResponse.xml", "") Then
                APISalesOrders = APIResponse("C:\Temp\APIResponse.xml")
                APIOriginalSalesOrders = APISalesOrders
                AppUtility.UI.Notification.CloseWaitForm()
                If APISalesOrders.Count > 0 Then
                    If AppUtility.UI.Notification.ConfirmMsg(APISalesOrders.Count & " Customer PO(s) found. Do you want view API Response data?") Then
                        LoadXmlContent()
                    End If
                    btnDuplicate.Enabled = True
                Else
                    MessageBox.Show("No Customer PO(s) found")
                End If
            End If

            'MessageBox.Show(responseStr)
            'Return responseStr
        End If
        AppUtility.UI.Notification.CloseWaitForm()
        ' Return Nothing


    End Sub
    Private Sub LoadXmlContent()
        Try
            Dim filePath As String = "C:\Temp\APIResponse.xml" ' Replace with the actual path to your XML file
            Dim xml As XElement = XElement.Load(filePath)
            Dim obj As New frmDisplayMessage
            obj.Show()
            obj.RichTextBox1.Text = xml.ToString()

        Catch ex As Exception
            MessageBox.Show("Error loading XML content: " & ex.Message)
        End Try
    End Sub
    Dim DupOrders As New List(Of String)
    Dim DupOrdersReason As New List(Of String)

    Dim ValidOrders As New List(Of String)
    Private Sub btnDuplicate_Click(sender As Object, e As EventArgs) Handles btnDuplicate.Click
        Dim objSyspro As New SysproObj
        Dim objPo As New PurchaseOrderMasterHdr
        Dim objPost As New PurchaseOrderBuild
        Dim obj As New frmValidate
        Dim objQry As New Query
        Dim sb As New StringBuilder

        Dim strFirstWarehouse As String = Nothing
        Dim intFirstExRate As Double = Nothing
        Dim strFirstBuyer As String = Nothing
        Dim strFirstSupplier As String = Nothing
        Dim strFirstRef As String = Nothing
        Dim intErrCount As Integer = 0
        If Trim(txtUser.Text) <> "" Then
            If Trim(txtComp.Text) <> "" Then
                _sysUsername = Trim(txtUser.Text)
                _sysPass = Trim(txtPass.Text)
                _sysComp = Trim(txtComp.Text)
                _sysCompPass = Trim(txtCompPass.Text)
                AppUtility.UI.Notification.ShowWaitForm(Me, "Checking Duplicates...")
                DupOrders.Clear()
                DupOrdersReason.Clear()
                ValidOrders.Clear()
                If Testlogin() Then
                    'Select Case optType.SelectedIndex
                    '    Case 0
                    sb.AppendLine("Duplicate PO(s) Found ")
                    sb.AppendLine("----------------------")
                    For Each dr In APISalesOrders
                        'Validate each line in the sheet as returned by dataset

                        If objPost.CheckDuplicateOrder(dr.PO) Then

                            sb.AppendLine(" This Purchase order number " & dr.PO & " has already be used for customer 1509 FWR cannot import.")
                            intErrCount = intErrCount + 1
                            DupOrders.Add(dr.PO)
                            DupOrdersReason.Add("Duplicate")
                        Else
                            ValidOrders.Add(dr.PO)
                        End If
                    Next

                    '    Case 1
                    '        ValidateNonStockedFile()
                    '    Case Else
                    '        MsgBox("Please Select either stocked or non stocked Option. Transaction Cancelled.", vbExclamation)
                    'End Select
                    ' _validated = True
                    'intErrCount = 0
                    AppUtility.UI.Notification.CloseWaitForm()
                    If intErrCount = 0 Then
                        btnValidate.Enabled = True
                        AppUtility.UI.Notification.InformationMsg("No Duplicate PO(s) Found")
                    Else
                        btnValidate.Enabled = False
                        sb.AppendLine("----------------------")
                        sb.AppendLine("Do you want to continue for other orders?")

                        If AppUtility.UI.Notification.ConfirmMsg(sb.ToString()) Then
                            APISalesOrders = APISalesOrders.Where(Function(c) ValidOrders.Contains(c.PO)).ToList()
                            btnValidate.Enabled = True
                        End If
                    End If

                End If
            Else
                MsgBox("Please enter the Sypro company", MsgBoxStyle.Exclamation)
                txtComp.Focus()
            End If
            AppUtility.UI.Notification.CloseWaitForm()
        Else
            MsgBox("Please enter your Syspro username", MsgBoxStyle.Exclamation)
            txtUser.Focus()
        End If
    End Sub
    Dim export = True
    Private Sub btnChangeStatus_Click(sender As Object, e As EventArgs) Handles btnChangeStatus.Click
        export = True
        AppUtility.UI.Notification.ShowWaitForm(Me, "Updating orders status to Exported...")
        Dim proceed = 1
        For Each item In APISalesOrders

            Try


                Dim xmlpath As String = getXML(True, item.PO)
                Dim API = "https://www.caravanrvcamping.com.au/do/WS/NetoAPI"
                Dim request As HttpWebRequest = DirectCast(WebRequest.Create(API), HttpWebRequest)
                Dim bytes() As Byte
                bytes = System.Text.Encoding.ASCII.GetBytes(xmlpath)
                request.ContentType = "application/xml"
                request.ContentLength = bytes.Length
                request.Method = "POST"
                request.Accept = "*/*"
                request.KeepAlive = True
                ' request.Headers.Add("Accept", "*/*")
                'request.Headers.Add("Accept-Encoding", "application/gzip,deflate,br")
                'request.Headers.Add("Connection", "keep-alive")
                request.Headers.Add("NETOAPI_ACTION", "GetOrder")
                request.Headers.Add("NETOAPI_USERNAME", "crcfeed")
                request.Headers.Add("NETOAPI_KEY", "nQzisw8oZ2hT5oGQDEgjFy1CuLXH172I")


                Dim requestStream As Stream = request.GetRequestStream()
                requestStream.Write(bytes, 0, bytes.Length)
                requestStream.Close()
                Using response As HttpWebResponse = DirectCast(request.GetResponse(), HttpWebResponse)
                    If response.StatusCode = HttpStatusCode.OK Then
                    Else
                        failedExportPO.Add(item.PO)
                        'MessageBox.Show(responseStr)
                        'Return responseStr
                    End If
                End Using

                'File.Delete("C:\Temp\APIResponse.xml")

            Catch ex As Exception
                failedExportPO.Add(item.PO)
                AppUtility.UI.Notification.CloseWaitForm()
                AppUtility.UI.Notification.ErrorMsg("Network error occured when updating orderstatus as exported, Please try again.")
            End Try
        Next
        AppUtility.UI.Notification.CloseWaitForm()
        If proceed = 1 Then
            AppUtility.UI.Notification.InformationMsg("Order Status updated as Exported")
            SimpleButton2.Enabled = True
            UpdateExportLog()
            AppUtility.UI.Notification.CloseWaitForm()
        End If

    End Sub
    Dim fromMail = "noreply@aussietraveller.com.au"
    Dim toMail = "phil@aussietraveller.com.au"
    Private Sub SimpleButton2_Click(sender As Object, e As EventArgs) Handles SimpleButton2.Click
        Try
            AppUtility.UI.Notification.ShowWaitForm(Me, "Sending Mail...")
            Dim message As New MailMessage()
            Dim smtp As New SmtpClient()

            message.From = New MailAddress("noreply@aussietraveller.com.au")
            message.To.Add(New MailAddress("phil@aussietraveller.com.au"))
            message.To.Add(New MailAddress("Sales@caravanrvcamping.com.au"))
            message.To.Add(New MailAddress("Samantha.Patterson@aussietraveller.com.au"))
            ' message.To.Add(New MailAddress("greg@codex-it.co.za"))
            'message.To.Add(New MailAddress("accounts@aussietraveller.com.au"))
            'message.CC.Add(New MailAddress("gordon.curley@aussietraveller.com.au"))
            'message.CC.Add(New MailAddress("Sue.Oughton@aussietraveller.com.au"))
            message.Subject = "FWR Orders successfully imported to Aussie Traveller Company " & _sysComp
            Dim body = "<b>Hello! </b>, <br> <br>The below list of orders have been successfully imported from your API. </br> Download the attached API Response data. <br/>"

            For i = 0 To SOResults.Count - 1
                body += "<span style='color:#f0ba3e; font-weight: bold;'>Order #: " & i + 1 & "</span>"
                body += "<table style='width:300px;align:center;color:#038a64;border: 1px solid black;border-collapse: collapse;'><th style='border: 1px solid black;border-collapse: collapse;'>Customer PO</th><th style='border: 1px solid black;border-collapse: collapse;'>Sales Order</th><th style='border: 1px solid black;border-collapse: collapse;'>Status</th>"
                body += "<tr style='border: 1px solid black;border-collapse: collapse;'><td style='border: 1px solid black;border-collapse: collapse;'>" & APISalesOrders(i).PO & "</td><td style='border: 1px solid black;border-collapse: collapse;'>" & SOResults(i) & "</td><td style='border: 1px solid black;border-collapse: collapse;'>&#10004;</td></tr>"
                body += "</table> <br/>"
                body += "<b>Details for the Salesorder " & SOResults(i) & "</b><br/>"

                body += "<table style='width:400px;color:blue;align:center'><th style='border: 1px solid black;border-collapse: collapse;'>Line#</th><th style='border: 1px solid black;border-collapse: collapse;'>SKU</th><th style='border: 1px solid black;border-collapse: collapse;'>Qty</th>"

                For Each item In APISalesOrders(i).SODtl
                    body += "<tr style='border: 1px solid black;border-collapse: collapse;'><td style='border: 1px solid black;border-collapse: collapse;'>" & item.LineNumber & "</td><td style='border: 1px solid black;border-collapse: collapse;'>" & item.ItemCode & "</td><td style='border: 1px solid black;border-collapse: collapse;'>" & item.Qty & "</td></tr>"
                Next
                body += "</table><br/>"

            Next
            body += "If you have any queries please contact Phil Mounfield at phil@aussietraveller.com.au"
            body += "<br><br><b> Thanks & Regards,</b> <br>Aussie Traveller."
            message.Body = body
            message.IsBodyHtml = True
            Dim attach As New Attachment("C:\Temp\APIResponse.xml")
            message.Attachments.Add(attach)
            smtp.Port = 587
            smtp.Host = "smtp.office365.com"
            smtp.EnableSsl = True
            smtp.UseDefaultCredentials = False
            smtp.Credentials = New NetworkCredential("noreply@aussietraveller.com.au", "wyxxrtprxclkczhx")
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network

            smtp.Send(message)
            AppUtility.UI.Notification.CloseWaitForm()
            AppUtility.UI.Notification.InformationMsg("Mail Sent Successfully")
            SimpleButton2.Enabled = False
            btnChangeStatus.Enabled = False
            attach.Dispose()
            UpdateMailLog()
        Catch ex As Exception
            MessageBox.Show("err: " & ex.Message)
            AppUtility.UI.Notification.CloseWaitForm()
        Finally
            If File.Exists("C:\Temp\APIResponse.xml") Then
                File.Delete("C:\Temp\APIResponse.xml")
            End If
        End Try
        AppUtility.UI.Notification.CloseWaitForm()

    End Sub
    'Private Sub btnCustomer_Click(sender As Object, e As EventArgs)
    '    _sysUsername = Trim(txtUser.Text)
    '    _sysPass = Trim(txtPass.Text)
    '    _sysComp = Trim(txtComp.Text)
    '    _sysCompPass = Trim(txtCompPass.Text)
    '    Using f As New BrowseCustomer()
    '        f.ShowDialog()
    '        If f.SelectedCustomer IsNot Nothing Then
    '            _arCustomer = f.SelectedCustomer
    '            lblCustomer.Text = _arCustomer.Customer
    '            lblCusname.Text = _arCustomer.Name
    '            lblWarehouse.Text = ""
    '        End If
    '    End Using
    'End Sub

    'Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles SimpleButton1.Click
    '    _sysUsername = Trim(txtUser.Text)
    '    _sysPass = Trim(txtPass.Text)
    '    _sysComp = Trim(txtComp.Text)
    '    _sysCompPass = Trim(txtCompPass.Text)
    '    Using f As New BrowseWareHouse()
    '        f.ShowDialog()
    '        If f.SelectedWarehouse IsNot Nothing Then
    '            _inWarehouse = f.SelectedWarehouse
    '            lblWarehouse.Text = _inWarehouse.Warehouse & "(" & _inWarehouse.Description & ")"
    '        End If
    '    End Using
    'End Sub

    Private Sub initializeform()
        btnPost.Enabled = False
        _sysUsername = ""
        _sysPass = ""
        _sysComp = ""
        _sysCompPass = ""
        Try
            lblVersion.Text = "Import SO - V" & System.Deployment.Application.ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString
            'Me.lblVersion.Caption = "Fire Co Reporting - V" & My.Application.Info.Version.ToString
        Catch inv As System.Deployment.Application.InvalidDeploymentException
            Me.lblVersion.Text = "Import SO - V" & My.Application.Info.Version.ToString
        End Try
    End Sub

    Private Function Testlogin() As Boolean
        Dim success As Boolean = False
        Dim s As New SysproObj
        If s.SysproConnect Then
            success = True
            s.LogOffSyspro()
        Else
            MsgBox(s.TrnMessage, MsgBoxStyle.Exclamation)
        End If
        Return success
    End Function

    Private Function ValidateNonStockedFile() As Boolean
        _validated = False
        Dim strFilePath As String = "" ' Trim(txtPath.Text)
        ' Insert code to read the stream here. (load dataset )
        Dim ds As DataSet = Nothing
        Dim dr As DataRow = Nothing
        Dim dt As DataTable = Nothing
        Dim intCount As Integer = 1
        'Dim dblOrdQty, dblPrice, dblExRate As Double
        ' Dim strStockcode, strSupplier As String
        Dim objSyspro As New SysproObj
        Dim objPo As New PurchaseOrderMasterHdr
        Dim objPost As New PurchaseOrderBuild
        Dim obj As New frmValidate
        Dim objQry As New Query
        Dim sb As New StringBuilder

        Dim strFirstWarehouse As String = Nothing
        Dim intFirstExRate As Double = Nothing
        Dim strFirstBuyer As String = Nothing
        Dim strFirstSupplier As String = Nothing
        Dim strFirstRef As String = Nothing
        Dim intErrCount As Integer = 0

        File.Delete("C:\Temp\StockCodeQry.xml")

        If strFilePath <> "" Then
            Try
                ds = Get_Excel(strFilePath)
            Catch ex As Exception
                MsgBox(ex.Message & vbNewLine & vbNewLine & " Please Check File path and try again.")
            End Try
            Try
                If IsNothing(ds) Then
                    Return _validated
                End If

                dt = ds.Tables(0)
                Dim intRCount As Integer = dt.Rows.Count

                intFirstExRate = ds.Tables(0).Rows(0).Item("Exchange Rate").ToString
                strFirstBuyer = ds.Tables(0).Rows(0).Item("Buyer").ToString
                strFirstSupplier = ds.Tables(0).Rows(0).Item("Supplier").ToString
                strFirstRef = ds.Tables(0).Rows(0).Item("OurRef").ToString

                obj.Show()

                sb.AppendLine("Errors found in Excel file to import:")
                sb.AppendLine("=======================================")
                sb.AppendLine()

                For Each dr In dt.Rows
                    'Validate each line in the sheet as returned by dataset
                    If dr("OrderQty") Is DBNull.Value Then
                        obj.ListBox1.Items.Add("OrderQty is empty on line " & intCount + 1)
                        sb.AppendLine("OrderQty is empty on line " & intCount + 1)
                        intErrCount = intErrCount + 1
                        obj.lblErr.Text = intErrCount
                    Else
                        'dblOrdQty = dr("OrderQty").ToString
                    End If

                    If dr("Price") Is DBNull.Value Then
                        obj.ListBox1.Items.Add("Price is empty on line " & intCount + 1)
                        sb.AppendLine("Price is empty on line " & intCount + 1)
                        intErrCount = intErrCount + 1
                        obj.lblErr.Text = intErrCount
                    Else
                        'dblPrice = dr("Price").ToString
                    End If

                    If dr("Buyer") Is DBNull.Value Then
                        obj.ListBox1.Items.Add("Buyer is empty on line " & intCount + 1)
                        sb.AppendLine("Buyer is empty on line " & intCount + 1)
                        intErrCount = intErrCount + 1
                        obj.lblErr.Text = intErrCount
                    ElseIf strFirstBuyer <> (dr("Buyer").ToString) Then
                        obj.ListBox1.Items.Add("Buyer on line " & intCount + 1 & " does not match the first Buyer.")
                        sb.AppendLine("Buyer on line " & intCount + 1 & " does not match the first Buyer.")
                        intErrCount = intErrCount + 1
                        obj.lblErr.Text = intErrCount
                    Else
                        ' strBuyer = dr("Buyer").ToString
                    End If

                    If dr("Supplier") Is DBNull.Value Then
                        obj.ListBox1.Items.Add("Supplier is empty on line " & intCount + 1)
                        sb.AppendLine("Supplier is empty on line " & intCount + 1)
                        intErrCount = intErrCount + 1
                        obj.lblErr.Text = intErrCount
                    ElseIf strFirstSupplier <> (dr("Supplier").ToString) Then
                        obj.ListBox1.Items.Add("Supplier on line " & intCount + 1 & " does not match the first Supplier.")
                        sb.AppendLine("Supplier on line " & intCount + 1 & " does not match the first Supplier.")
                        intErrCount = intErrCount + 1
                        obj.lblErr.Text = intErrCount
                    End If

                    If dr("Exchange Rate") Is DBNull.Value Then
                        obj.ListBox1.Items.Add("Exchange Rate is empty on line " & intCount + 1)
                        sb.AppendLine("Exchange Rate is empty on line " & intCount + 1)
                        intErrCount = intErrCount + 1
                        obj.lblErr.Text = intErrCount
                    ElseIf intFirstExRate <> (dr("Exchange Rate").ToString) Then
                        obj.ListBox1.Items.Add("Exchange Rate on line " & intCount + 1 & " does not match the first Exchange rate.")
                        sb.AppendLine("Exchange Rate on line " & intCount + 1 & " does not match the first Exchange rate.")
                        intErrCount = intErrCount + 1
                        obj.lblErr.Text = intErrCount
                    Else
                        'dblExRate = dr("Exchange Rate").ToString
                    End If

                    If dr("OurRef") Is DBNull.Value Then
                        obj.ListBox1.Items.Add("OurRef is empty on line " & intCount + 1)
                        sb.Append("OurRef is empty on line " & intCount + 1)
                        intErrCount = intErrCount + 1
                        obj.lblErr.Text = intErrCount
                    End If

                    If dr("ProductClass") Is DBNull.Value Then
                        obj.ListBox1.Items.Add("ProductClass is empty on line " & intCount + 1)
                        sb.Append("ProductClass is empty on line " & intCount + 1)
                        intErrCount = intErrCount + 1
                        obj.lblErr.Text = intErrCount
                    End If

                    If dr("GlCode") Is DBNull.Value Then
                        obj.ListBox1.Items.Add("Optional GlCode is empty on line " & intCount + 1)
                        sb.Append("Optional GlCode is empty on line " & intCount + 1)
                        'intErrCount = intErrCount + 1
                    End If

                    Using outfile As New StreamWriter("C:\Temp\ValidateErrs.txt")
                        outfile.Write(sb.ToString())
                    End Using
                    intCount += 1
                Next
            Catch ex As Exception
                MsgBox(ex.Message & " Please check file and try again.")
                Return _validated
            End Try

            If intErrCount < 1 Then
                obj.ListBox1.Items.Add("No Errors found in file")
                ' btnPost.Enabled = True
                _validated = True
            End If
        Else
            MsgBox("Please enter or browse for a valid file path", , "Import PO")
        End If

        Return _validated

    End Function

    Private Sub Build_StockedDetail(ByRef objPo As PurchaseOrderMasterHdr, intcount As Integer, strStockcode As String, dblOrdQty As Decimal,
                                    dblPrice As Decimal, dblExRate As Decimal, strWarehouse As String,
                                    commObj As PurchaseOrderCommentLine, supplierCatalogue As String, shipDate As Date)

        objPo.PurchaseOrderDetails.Add(New PurchaseOrderDetails(intcount, strStockcode,
                                                 dblOrdQty, Math.Round((dblPrice * dblExRate), 2), "03", strWarehouse, commObj, supplierCatalogue, shipDate))

    End Sub


    Private Sub Build_NonStockedDetail(ByRef objPo As PurchaseOrderMasterHdr, intcount As Integer, strStockcode As String, dblOrdQty As Decimal,
                                    dblPrice As Decimal, dblExRate As Decimal, strWarehouse As String, description As String,
                                    glcode As String, pClass As String, commObj As PurchaseOrderCommentLine, supplierCata As String, shipDate As Date)

        objPo.PurchaseOrderDetails.Add(New PurchaseOrderDetails(intcount, strStockcode,
                                                 dblOrdQty, Math.Round((dblPrice * dblExRate), 2), pClass, strWarehouse, description, glcode,
                                                                commObj, supplierCata, shipDate))

    End Sub

End Class