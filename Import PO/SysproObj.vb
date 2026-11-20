Imports System.Text
Imports System.IO
Imports System.Xml

Public Class SysproObj

    Private _trnMessage As String = ""
    Public ReadOnly Property TrnMessage As String
        Get
            Return _trnMessage
        End Get
    End Property
    Private Sub AppendTrnMessage(msg As String)
        If _trnMessage IsNot Nothing Then
            _trnMessage &= Environment.NewLine
        End If
        _trnMessage &= msg
    End Sub

    'Change as appropriate
    'WebService Syspro 
    'Must also change logon Userid in SysproConnect
    Public Qry As New SysproQueryService.queryclass
    ' Public Qry As New Encore.Query
    Public Tran As New SysproTransService.transactionclass
    ' Public Tran As New Encore.Transaction
    Public Util As New SysproUtilitiesService.utilitiesclass
    ' Public Util As New Encore.Utilities
    Public stp As New SysproSetupService.setupclass
    ' Public stp As New Encore.Setup
    'Local
    Public UserID As String = Nothing
    'Public Qry As New Encore.Query  'Encore local system
    'Public Util As New Encore.Utilities
    'Public Tran As New Encore.Transaction
    'Public stp As New Encore.Setup

    Public Function SysproConnect() As Boolean
        'Dim Util As New SysproUtilitiesService.utilitiesclass
        SysproConnect = False
        UserID = Nothing
        Try

            UserID = Util.Logon(_sysUsername, _sysPass, _sysComp, _sysCompPass, 5, 0, 0, "")
        Catch exc As Exception
            AppendTrnMessage(exc.Message)
            'MessageBox.Show("Exception - message:" & exc.Message)
            Return False
        End Try
        Return True

    End Function

    Public Function LogOffSyspro()
        ' Dim Util As New SysproUtilitiesService.utilitiesclass
        Dim ReturnCode As Long
        Try
            ReturnCode = Util.Logoff(UserID)
        Catch ex As Exception
            AppendTrnMessage(ex.Message)
            'MsgBox(ex.Message)
            Return ReturnCode
            Exit Function
        End Try
        Return ReturnCode

    End Function

    Public Function postSysproXML(ByVal xmlin As String, ByVal xmlParam As String, ByVal BusinessObj As String, ByVal path As String) As Boolean

        Dim xmlOut As String = Nothing
        Dim strMsg As String = Nothing
        If SysproConnect() = True Then
            ' invoking business object 
            Try
                xmlOut = Tran.Post(UserID, BusinessObj, xmlParam, xmlin)
            Catch exc As Exception

                strMsg = "transacion warning: " & exc.Message & " Data Not Posted. Post Not Committed"
            End Try

            If Not (xmlOut Is Nothing) Then
                Dim XMLfile As Boolean = SaveTextToFile(xmlOut, path, "")

            Else
                MsgBox("Xml returned nothing" & vbNewLine & vbNewLine & strMsg)
                LogOffSyspro()
                Tran = Nothing
                Util = Nothing
                Return False
            End If

            LogOffSyspro()
            Tran = Nothing
            Util = Nothing
            Return True
        Else
            MsgBox(_trnMessage, MsgBoxStyle.Exclamation)
        End If

        Return False

    End Function

    Public Function SetupSyspro_xml(ByVal xmlin As String, ByVal xmlParam As String, ByVal BusinessObj As String, ByVal path As String)
        Dim xmlOut As String = Nothing
        Dim strMsg As String = Nothing
        If SysproConnect() = True Then
            ' invoking business object 
            Try
                xmlOut = stp.Add(UserID, BusinessObj, xmlParam, xmlin)
            Catch exc As Exception

                strMsg = "Setup warning: " & exc.Message & " Data Not Posted. Post Not Committed"
            End Try

            If Not (xmlOut Is Nothing) Then
                Dim XMLfile As Boolean = SaveTextToFile(xmlOut, path, "")

            Else
                MsgBox("Xml returned nothing" & vbNewLine & vbNewLine & strMsg)
                LogOffSyspro()
                Tran = Nothing
                Util = Nothing
                Return False
            End If

            LogOffSyspro()
            Tran = Nothing
            Util = Nothing
            Return True
        Else
            MsgBox(_trnMessage, MsgBoxStyle.Exclamation)
        End If

        Return False
    End Function

    Public Function Read_Xml(ByVal xml_Path As String) As String

        Dim tr As XmlTextReader = New XmlTextReader(xml_Path)
        Dim doc As New XmlDocument
        Dim POnum As String = Nothing
        Dim PoLine As String = Nothing
        Dim GrnNumber As String = Nothing
        Dim sbWH As New StringBuilder
        doc.Load(xml_Path)
        tr.Read()
        While tr.Read
            If tr.NodeType.Equals(XmlNodeType.Element) AndAlso tr.Name.Equals("ErrorDescription") AndAlso tr.IsStartElement() Then
                Dim strXMLErr As String = tr.ReadString
                tr.Close()
                Return MsgBox("SYSPRO MESSAGE: " & vbNewLine & vbNewLine &
                        strXMLErr, MsgBoxStyle.Critical).ToString
            End If
        End While

        If xml_Path = "C:\Temp\POR.xml" Then
            Try
                For Each po As XmlElement In doc.SelectNodes("postpurchaseorders/Order/Key")
                    POnum = po.SelectSingleNode("PurchaseOrder").InnerText
                Next
                Return MsgBox("Done Posting:" & vbNewLine & vbNewLine & POnum, MsgBoxStyle.Information, "Created PO")
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        ElseIf xml_Path = "C:\Temp\SOR.xml" Then
            Try
                For Each po As XmlElement In doc.SelectNodes("SalesOrders/Order")
                    POnum = po.SelectSingleNode("SalesOrder").InnerText
                Next
                tr.Close()
                SOResults.Add(POnum)
                Return POnum
            Catch ex As Exception
                tr.Close()
                MsgBox(ex.Message)

            End Try
        ElseIf xml_Path = "C:\Temp\Stock.xml" Then
            Try
                For Each po As XmlElement In doc.SelectNodes("InvQuery/StockItem")
                    POnum = po.SelectSingleNode("StockUom").InnerText
                Next
                tr.Close()

                Return POnum
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        ElseIf xml_Path = "C:\Temp\PorReceipts.xml" Then
            Try

                For Each po As XmlElement In doc.SelectNodes("postpurchaseorderreceipts/Item/Receipt")
                    POnum = po.SelectSingleNode("PurchaseOrder").InnerText
                    GrnNumber = po.SelectSingleNode("Grn").InnerText
                    PoLine = po.SelectSingleNode("PurchaseOrderLine").InnerText
                Next
                Return MsgBox("Done Posting:" & vbNewLine & vbNewLine &
                                                  "PO: " & POnum & vbNewLine & vbNewLine &
                                                 "PO Line: " & PoLine & vbNewLine & vbNewLine &
                                                  "Grn Created: " & GrnNumber, MsgBoxStyle.Information, "POR Receipt")
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try

        ElseIf xml_Path = "C:\Temp\SorInv.xml" Then
            Try
                For Each po As XmlElement In doc.SelectNodes("postsalesorderinvoice/Item/Key")
                    POnum = po.SelectSingleNode("InvoiceNumber").InnerText
                    GrnNumber = po.SelectSingleNode("Register").InnerText

                Next
                Return MsgBox("Invoice Created:" & vbNewLine & vbNewLine &
                                                  "Invoice: " & POnum & vbNewLine & vbNewLine &
                                                  "Register: " & GrnNumber _
                                                  & vbNewLine & vbNewLine & "Path: " & xml_Path,
                                                  MsgBoxStyle.Information, "Sales Order Invoice")

            Catch ex As Exception
                MsgBox(ex.Message, , "Invoice Creation")
            End Try

        ElseIf xml_Path = "C:\Temp\StockCodeQry.xml" Then
            Try
                For Each wh As XmlElement In doc.SelectNodes("InvQuery/WarehouseItem")
                    sbWH.AppendLine(wh.SelectSingleNode("Warehouse").InnerText.ToString)
                Next
                Return sbWH.ToString

            Catch ex As Exception
                MsgBox(ex.Message, , "Stock Code Query")
            End Try

        End If

        Return MsgBox("Done Posting:", MsgBoxStyle.Information)

    End Function

    Public Function QuerySyspro(ByVal xmlin As String, ByVal BusinessObj As String, ByVal path As String) As Boolean
        ' Dim Qry As New SysproQueryService.queryclass
        Dim xmlOut As String = Nothing
        Dim strMsg As String = Nothing
        If SysproConnect() = True Then
            ' invoking business object 
            Try
                xmlOut = Qry.Query(UserID, BusinessObj, xmlin)
            Catch exc As Exception

                'strQryMsg = exc.Message & ". Query Failed"
                AppendTrnMessage(exc.Message)
            End Try

            If Not (xmlOut Is Nothing) Then
                Dim XMLfile As Boolean = SaveTextToFile(xmlOut, path, "")

            Else
                'MsgBox("Xml returned nothing" & vbNewLine & vbNewLine & strMsg)
                LogOffSyspro()
                Qry = Nothing
                Util = Nothing
                Return Nothing
            End If

            LogOffSyspro()
            Qry = Nothing
            Util = Nothing
            Return True
        End If

        Return False

    End Function
    Public Function FillWarehouse(ByVal xml_Path As String) As List(Of InvWhControlDTO)
        Dim listofCustomer = New List(Of InvWhControlDTO)
        Dim tr As XmlTextReader = New XmlTextReader(xml_Path)
        Dim doc As New XmlDocument
        Dim sbWH As New StringBuilder
        doc.Load(xml_Path)
        tr.Read()
        For Each po As XmlElement In doc.SelectNodes("InvWhControl/Row")
            Dim customer = New InvWhControlDTO()
            customer.Warehouse = po.SelectSingleNode("Warehouse").InnerText
            customer.Description = po.SelectSingleNode("Description").InnerText
            listofCustomer.Add(customer)
        Next
        tr.Close()
        Return listofCustomer
    End Function
    Public Function FillCustomer(ByVal xml_Path As String) As List(Of ARCustomerDTO)
        Dim listofCustomer = New List(Of ARCustomerDTO)
        Dim tr As XmlTextReader = New XmlTextReader(xml_Path)
        Dim doc As New XmlDocument
        Dim sbWH As New StringBuilder
        doc.Load(xml_Path)
        tr.Read()
        For Each po As XmlElement In doc.SelectNodes("ArCustomer/Row")
            Dim customer = New ARCustomerDTO()
            customer.Customer = po.SelectSingleNode("Customer").InnerText
            customer.Name = po.SelectSingleNode("Name").InnerText
            listofCustomer.Add(customer)
        Next
        tr.Close()
        Return listofCustomer
    End Function
End Class
