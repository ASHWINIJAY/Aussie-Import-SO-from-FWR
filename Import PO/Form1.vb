Imports System.IO
Imports System.Text

Public Class frmImport

    Private Sub cmdPost_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdPost.Click
        Dim strFilePath As String = Trim(txtPath.Text)
        ' Insert code to read the stream here. (load dataset )
        Dim ds As DataSet = Nothing
        Dim dr As DataRow = Nothing
        Dim dt As DataTable = Nothing
        Dim intCount As Integer = 1
        Dim dblOrdQty, dblPrice, dblExRate As Double
        Dim strStockcode, strRef, strSupplier, strBuyer, strWarehouse As String
        Dim objSyspro As New SysproObj
        Dim objPo As New PurchaseOrderMasterHdr
        Dim objPost As New PurchaseOrderBuild

        If strFilePath <> "" Then
            Try
                ds = Get_Excel(strFilePath)
            Catch ex As Exception
                MsgBox(ex.Message & vbNewLine & vbNewLine & " Please Check File path and try again.")
            End Try

            If IsNothing(ds) Then
                Exit Sub
            End If

            dt = ds.Tables(0)
            Dim intRCount As Integer = dt.Rows.Count

            'build PO header
            With objPo

                strRef = dt.Rows(0)("OurRef").ToString
                strSupplier = dt.Rows(0)("Supplier").ToString
                strBuyer = dt.Rows(0)("Buyer").ToString
                .Ref = strRef
                .Supplier = strSupplier
                .Buyer = strBuyer

            End With

            'build PO details 
            For Each dr In dt.Rows
                'add code to create po line
                Try
                    If (dr("StockCode")) Is DBNull.Value Then
                        MsgBox("StockCode is empty on line " & intCount + 1 & ". Please check the file and re-import. Import terminated.")
                        Exit Sub
                    Else
                        strStockcode = dr("StockCode").ToString
                    End If

                    If dr("Warehouse") Is DBNull.Value Then
                        MsgBox("Warehouse is empty on line " & intCount + 1 & ". Please check the file and re-import. Import terminated.")
                        Exit Sub
                    Else
                        strWarehouse = dr("Warehouse").ToString
                    End If

                    If dr("OrderQty") Is DBNull.Value Then
                        MsgBox("OrderQty is empty on line " & intCount + 1 & ". Please check the file and re-import. Import terminated.")
                        Exit Sub
                    Else
                        dblOrdQty = dr("OrderQty").ToString
                    End If

                    If dr("Price") Is DBNull.Value Then
                        MsgBox("Price is empty on line " & intCount + 1 & ". Please check the file and re-import. Import terminated.")
                        Exit Sub
                    Else
                        dblPrice = dr("Price").ToString
                    End If

                    If dr("Buyer") Is DBNull.Value Then
                        MsgBox("Buyer is empty on line " & intCount + 1 & ". Please check the file and re-import. Import terminated.")
                        Exit Sub
                    Else
                        strBuyer = dr("Buyer").ToString
                    End If

                    If dr("Supplier") Is DBNull.Value Then
                        MsgBox("Supplier is empty on line " & intCount + 1 & ". Please check the file and re-import. Import terminated.")
                        Exit Sub
                    Else
                        strSupplier = dr("Supplier").ToString
                    End If

                    If dr("Exchange Rate") Is DBNull.Value Then
                        MsgBox("Exchange Rate is empty on line " & intCount + 1 & ". Please check the file and re-import. Import terminated.")
                        Exit Sub
                    Else
                        dblExRate = dr("Exchange Rate").ToString
                    End If

                    If dr("OurRef") Is DBNull.Value Then
                        MsgBox("Our Ref is empty on line " & intCount + 1 & ". Please check the file and re-import. Import terminated.")
                        Exit Sub
                    Else
                        strRef = dr("OurRef").ToString
                    End If
                Catch ex As Exception
                    MsgBox(ex.Message & " Please check file and try again.")
                    Exit Sub
                End Try

                'objPo.PurchaseOrderDetails.Add(New PurchaseOrderDetails(intCount, strStockcode, _
                '                                dblOrdQty, Math.Round((dblPrice * dblExRate), 2), "03", strWarehouse))
                intCount = intCount + 1
            Next
            ' objPost.post_PurchaseOrder(objPo, 0)
        Else
            MsgBox("Please enter or browse for a valid file path", , "Import PO")
            Exit Sub
        End If

    End Sub

    Private Sub cmdBrowse_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdBrowse.Click
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
                    txtPath.Text = openFileDialog1.FileName
                End If
            Catch Ex As Exception
                MessageBox.Show("Cannot read file from disk. Original error: " & Ex.Message)
            Finally
                ' Check this again, since we need to make sure we didn't throw an exception on open. 
                If (myStream IsNot Nothing) Then
                    myStream.Close()
                    cmdPost.Enabled = False
                End If
            End Try
        End If

    End Sub

    Private Sub cmdValidate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdValidate.Click
        Dim strFilePath As String = Trim(txtPath.Text)
        ' Insert code to read the stream here. (load dataset )
        Dim ds As DataSet = Nothing
        Dim dr As DataRow = Nothing
        Dim dt As DataTable = Nothing
        Dim intCount As Integer = 1
        'Dim dblOrdQty, dblPrice, dblExRate As Double
        Dim strStockcode, strSupplier As String
        Dim objSyspro As New SysproObj
        Dim objPo As New PurchaseOrderMasterHdr
        Dim objPost As New PurchaseOrderBuild
        Dim obj As New Form2
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
                    Exit Sub
                End If

                dt = ds.Tables(0)
                Dim intRCount As Integer = dt.Rows.Count

                strFirstWarehouse = ds.Tables(0).Rows(0).Item("Warehouse").ToString
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

                    If (dr("StockCode")) Is DBNull.Value Then
                        ' Add statement to report and continue
                        obj.ListBox1.Items.Add("StockCode is empty on line " & intCount + 1)
                        'append text file line
                        sb.AppendLine("StockCode is empty on line " & intCount + 1)
                        intErrCount = intErrCount + 1
                        obj.lblErr.Text = intErrCount
                    ElseIf (dr("StockCode")) IsNot DBNull.Value Then
                        strStockcode = dr("StockCode").ToString
                        ''Check if stock exists in syspro
                        'If objSyspro.QuerySyspro(objQry.QryStockCode_xml(strStockcode), "INVQRY", "C:\Temp\StockCodeQry.xml") Then
                        '    If InStr(objSyspro.Read_Xml("C:\Temp\StockCodeQry.xml").ToString(), dr("Warehouse").ToString) Then
                        '    Else
                        '        obj.ListBox1.Items.Add("Warehouse " & dr("Warehouse").ToString & " is not a warehouse for this stockcode. Line " & intCount + 1)
                        '        'append text file line
                        '        sb.AppendLine("Warehouse " & dr("Warehouse").ToString & " is not a warehouse for this stockcode. Line " & intCount + 1)
                        '        'error message to box
                        '    End If

                        'Else
                        '    obj.ListBox1.Items.Add("StockCode query: Line " & intCount + 1 & " Syspro Msg: " & strQryMsg)
                        '    sb.AppendLine("StockCode query: Line " & intCount + 1 & " Syspro Msg: " & strQryMsg)
                        '    strQryMsg = Nothing
                        '    intErrCount = intErrCount + 1
                        '    obj.lblErr.Text = intErrCount
                        'End If
                    End If

                    If dr("Warehouse") Is DBNull.Value Then
                        obj.ListBox1.Items.Add("Warehouse is empty on line " & intCount + 1)
                        sb.AppendLine("Warehouse is empty on line " & intCount + 1)
                        intErrCount = intErrCount + 1
                        obj.lblErr.Text = intErrCount
                        'check if warehouse is the same as the first one entered
                    ElseIf strFirstWarehouse <> (dr("Warehouse").ToString) Then
                        obj.ListBox1.Items.Add("Warehouse on line " & intCount + 1 & " does not match the first Warehouse.")
                        sb.AppendLine("Warehouse on line " & intCount + 1 & " does not match the first Warehouse.")
                        intErrCount = intErrCount + 1
                        obj.lblErr.Text = intErrCount
                    Else
                        'strWarehouse = dr("Warehouse").ToString
                    End If

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
                    ElseIf dr("Supplier") IsNot DBNull.Value Then
                        strSupplier = dr("Supplier").ToString
                        If objSyspro.QuerySyspro(objQry.QrySupplier_xml(strSupplier), "APSQRY", "C:\Temp\SupplierCodeQry.xml") Then
                            'objSyspro.Read_Xml("C:\Temp\SupplierCodeQry.xml")
                        Else
                            obj.ListBox1.Items.Add("Supplier query: Line " & intCount + 1 & " Syspro Msg: " & strQryMsg)
                            sb.AppendLine("Supplier query: Line " & intCount + 1 & " Syspro Msg: " & strQryMsg)
                            strQryMsg = Nothing
                            intErrCount = intErrCount + 1
                            obj.lblErr.Text = intErrCount
                        End If
                    Else
                        'strSupplier = dr("Supplier").ToString
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
                    ElseIf strFirstRef <> (dr("OurRef").ToString) Then
                        obj.ListBox1.Items.Add("Reference on line " & intCount + 1 & " does not match the first reference.")
                        sb.AppendLine("Reference on line " & intCount + 1 & " does not match the first reference.")
                        intErrCount = intErrCount + 1
                        obj.lblErr.Text = intErrCount
                    Else
                        ' strRef = dr("OurRef").ToString
                    End If

                    Using outfile As New StreamWriter("C:\Temp\ValidateErrs.txt")
                        outfile.Write(sb.ToString())
                    End Using
                    intCount += 1
                Next
            Catch ex As Exception
                MsgBox(ex.Message & " Please check file and try again.")
                Exit Sub
            End Try
        Else
            MsgBox("Please enter or browse for a valid file path", , "Import PO")
            Exit Sub
        End If

        If obj.ListBox1.Items.Count = Nothing Then
            obj.ListBox1.Items.Add("No Errors found in file")
            cmdPost.Enabled = True
        End If

    End Sub

    Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        cmdPost.Enabled = False
    End Sub

    'Public Function CheckCodeExists(ByVal stockcode As String)
    '    Dim objQry As New Query
    '    Dim sysObj As New SysproObj

    '    sysObj.SysproConnect()

    '    Return True
    'End Function

    Private Sub cmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClose.Click
        End
    End Sub

End Class
