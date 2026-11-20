

Imports System.Collections.Generic
Imports System.Xml
Imports System.Text
Imports System.Data.SqlClient

Public Class PurchaseOrderMasterHdr
    Public Property Supplier As String
    Public Property OrderDater As Date
    Public Property DueDate As Date
    Public Property ShipDate As Date
    Public Property Buyer As String
    Public Property Ref As String
    Public Property DeliveryAddress As String
    Public Property DeliveryAddress1 As String
    Public Property DeliveryAddress2 As String
    Public Property DeliveryAddress3 As String
    Public Property PurchaseOrderDetails As New List(Of PurchaseOrderDetails)

End Class
Public Class SOHdr
    Public Property PO As String
    Public Property OrderDate As Date
    Public Property ShipDate As Date
    Public Property MultiShipCode As String
    Public Property Warehouse As String
    Public Property SODtl As New List(Of SODtl)
    Public Property CustomerCode As String
    Public Property CustomerName As String
    Public Property ShipAddr1 As String
    Public Property ShipAddr2 As String
    Public Property ShipAddr3 As String
    Public Property ShipZip As String
    Public Property ShipPhone As String

End Class
Public Class POSite
    Public Property site As String
    Public Property PO As String
    Public Property Warehouse As String
    Public Property LineNumber As Integer
    Public Property ItemCode As String
    Public Property Qty As Integer
    Public Property UnitPrice As Decimal
    Public Property shipdate As Date
    Public Property OrderDate As Date
End Class
Public Class SODtl
    Private intcount As Integer
    Private stockcode As String
    Private qty1 As String
    Private price As String

    Public Sub New(intcount As Integer, stockcode As String, qty1 As String, price As Decimal, shipdate As Date, _UOM As String, _warehouse As String, _DiscPer As Decimal)
        Me.LineNumber = intcount
        Me.ItemCode = stockcode
        Me.Qty = qty1
        Me.UnitPrice = price
        Me.shipdate = shipdate
        Me.UOM = _UOM
        Me.Warehouse = _warehouse
        Me.DiscPer = _DiscPer
    End Sub

    Public Property LineNumber As Integer
    Public Property ItemCode As String
    Public Property Warehouse As String
    Public Property UOM As String
    Public Property Qty As Integer
    Public Property UnitPrice As Decimal
    Public Property shipdate As Date
    Public Property DiscPer As Decimal
End Class
Public Class PurchaseOrderDetails

    Public Property LineNumber As Integer
    Public Property ItemCode As String
    Public Property Description As String
    Public Property Qty As Integer
    Public Property UnitPrice As Decimal
    Public Property ProductClass As String
    Public Property warehouse As String
    Public Property glCode As String
    Public Property Comment As PurchaseOrderCommentLine
    Public Property SupplierCat As String
    Public Property ShipDate As Date

    Public Sub New(ByVal DetailLine As Integer, ByVal ItemCode As String, ByVal Qty As Integer,
                   ByVal UnitPrice As Decimal, ByVal PClass As String, ByVal wh As String, _comment As PurchaseOrderCommentLine, supcat As String, shipDate As Date)
        Me.LineNumber = DetailLine
        Me.ItemCode = ItemCode
        Me.Qty = Qty
        Me.UnitPrice = UnitPrice
        Me.ProductClass = PClass
        Me.warehouse = wh
        If _comment IsNot Nothing Then
            Me.Comment = _comment
        End If
        Me.SupplierCat = supcat
        Me.ShipDate = shipDate
    End Sub

    Public Sub New(ByVal DetailLine As Integer, ByVal ItemCode As String, ByVal Qty As Integer,
                   ByVal UnitPrice As Double, ByVal PClass As String, ByVal wh As String,
                   description As String, glcode As String, _comment As PurchaseOrderCommentLine, supCat As String, shipDate As Date)
        Me.LineNumber = DetailLine
        Me.ItemCode = ItemCode
        Me.Description = description
        Me.Qty = Qty
        Me.UnitPrice = UnitPrice
        Me.ProductClass = PClass
        Me.warehouse = wh
        Me.glCode = glcode
        If _comment IsNot Nothing Then
            Me.Comment = _comment
        End If
        Me.SupplierCat = supCat
        Me.ShipDate = shipDate
    End Sub

End Class

Public Class PurchaseOrderCommentLine
    Public Property LineNum As Integer
    Public Property Comment As String

    Public Sub New(ByVal LineNumber As Integer, ByVal Comment As String)
        Me.LineNum = LineNumber
        Me.Comment = Comment
    End Sub
End Class

Public Class PurchaseOrderBuild
    Public Function getUOM(stockcode As String)
        Dim connString = System.Configuration.ConfigurationManager.ConnectionStrings("SqlConn").ConnectionString
        If _sysComp = "Z" Then
            connString = connString.Replace("SysproCompanyN", "SysproCompanyZ")
        End If

        Dim con As New SqlConnection(connString)


        Dim sqlCommand As New SqlCommand("select * from  InvMaster where StockCode='" & stockcode & "'", con)

        con.Open()
        Dim SqlAda = New SqlDataAdapter(sqlCommand)
        Dim ds = New DataSet()
        SqlAda.Fill(ds)
        con.Close()
        If ds.Tables(0).Rows.Count > 0 Then
            Return ds.Tables(0).Rows(0)("StockUom")
        Else
            Return "Failed"
        End If
    End Function
    Public Function getStock(stockcode As String)
        Dim connString = System.Configuration.ConfigurationManager.ConnectionStrings("SqlConn").ConnectionString
        If _sysComp = "Z" Then
            connString = connString.Replace("SysproCompanyN", "SysproCompanyZ")
        End If

        Dim con As New SqlConnection(connString)


        Dim sqlCommand As New SqlCommand("select * from  InvMaster where StockCode='" & stockcode & "'", con)

        con.Open()
        Dim SqlAda = New SqlDataAdapter(sqlCommand)
        Dim ds = New DataSet()
        SqlAda.Fill(ds)
        con.Close()
        If ds.Tables(0).Rows.Count > 0 Then
            If ds.Tables(0).Rows(0)("StockOnHold").ToString().Trim() = "" Then
                Return True
            End If
            Return False
        Else
            Return False
        End If
    End Function
    Public Function getWarehouse(stockcode As String)
        Dim connString = System.Configuration.ConfigurationManager.ConnectionStrings("SqlConn").ConnectionString
        If _sysComp = "Z" Then
            connString = connString.Replace("SysproCompanyN", "SysproCompanyZ")
        End If

        Dim con As New SqlConnection(connString)


        Dim sqlCommand As New SqlCommand("select * from  InvWarehouse where Warehouse='40' and StockCode='" & stockcode & "'", con)

        con.Open()
        Dim SqlAda = New SqlDataAdapter(sqlCommand)
        Dim ds = New DataSet()
        SqlAda.Fill(ds)
        con.Close()
        If ds.Tables(0).Rows.Count > 0 Then
            Return True
        Else
            Return False
        End If
    End Function
    Public Function CheckDuplicateOrder(Order As String)
        Dim connString = System.Configuration.ConfigurationManager.ConnectionStrings("SqlConn").ConnectionString
        If _sysComp = "Z" Then
            connString = connString.Replace("SysproCompanyN", "SysproCompanyZ")
        End If

        Dim con As New SqlConnection(connString)


        Dim sqlCommand As New SqlCommand("select * from  SorMaster where CustomerPoNumber='" & Order & "'", con)

        con.Open()
        Dim SqlAda = New SqlDataAdapter(sqlCommand)
        Dim ds = New DataSet()
        SqlAda.Fill(ds)
        con.Close()
        If _sysUsername = "XBS" Then
            Return False
        End If
        If ds.Tables(0).Rows.Count > 0 Then

            Return True
        Else
            Return False
        End If
    End Function
    Public Function getUnitPrice(stockcode As String)
        Dim connString = System.Configuration.ConfigurationManager.ConnectionStrings("SqlConn").ConnectionString
        If _sysComp.ToUpper() = "Z" Then
            connString = connString.Replace("SysproCompanyN", "SysproCompanyZ")
        End If
        Dim con As New SqlConnection(connString)


        Dim sqlCommand As New SqlCommand("select FixedPrice from  SorContractPrice where StockCode='" & stockcode & "' and  CustomerBuyGrp='F2' and ExpiryDate>=getdate()", con)

        con.Open()
        Dim SqlAda = New SqlDataAdapter(sqlCommand)
        Dim ds = New DataSet()
        SqlAda.Fill(ds)
        con.Close()
        If ds.Tables(0).Rows.Count > 0 Then
            Return ds.Tables(0).Rows(0)("FixedPrice")
        Else
            con.Open()
            sqlCommand = New SqlCommand("select PriceCode from  ArCustomer where Customer like'%1509'", con)


            SqlAda = New SqlDataAdapter(sqlCommand)
            ds = New DataSet()
            SqlAda.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then

            Else
                Return -0.1
            End If
            sqlCommand = New SqlCommand("select SellingPrice from  InvPrice where PriceCode='" & ds.Tables(0).Rows(0)(0) & "' and StockCode='" & stockcode & "'", con)


            SqlAda = New SqlDataAdapter(sqlCommand)
            ds = New DataSet()
            SqlAda.Fill(ds)
            con.Close()
            If ds.Tables(0).Rows.Count > 0 Then
                Return ds.Tables(0).Rows(0)("SellingPrice")
            Else
                Return -0.1
            End If
        End If
    End Function
    Public Function getUOM1(stockcode As String)
        Dim Document As New System.Text.StringBuilder
        With Document
            .Append("<?xml version=""1.0"" encoding=""Windows-1252""?>")
            .Append("<Query xmlns:xsd=""http://www.w3.org/2000/10/XMLSchema-instance"" xsd:noNamespaceSchemaLocation=""INVQRY.XSD"">")
            .Append("<Key>")
            .Append("<StockCode>" & stockcode & "</StockCode>")
            .Append("</Key>")
            .Append("</Query>")
        End With
        Dim objSys As New SysproObj
        If objSys.QuerySyspro(Document.ToString, "INVQRY", "C:\Temp\Stock.xml") Then
            Return objSys.Read_Xml("C:\Temp\Stock.xml")
            'do po query and count lines posted and return message to user
            'Return True
        Else
            ' MsgBox("Failed")
            Return "Failed"
        End If
    End Function
    Public Function post_SOOrder(ByVal por As SOHdr)

        Dim xmlIn As New System.Text.StringBuilder
        Dim xmlParam As New StringBuilder

        'Create header
        xmlIn.Append(SOHeader_xml(por))

        ''insert detail lines

        For Each item In por.SODtl
            xmlIn.Append(SOStockLine_xml(item))

        Next


        xmlIn.Append(SO_Footer_xml)
        xmlParam.Append(SoParam_xml())
        Dim str As String = xmlIn.ToString
        Dim objSys As New SysproObj

        If objSys.postSysproXML(xmlIn.ToString, xmlParam.ToString, "SORTOI", "C:\Temp\SOR.xml") Then
            Return objSys.Read_Xml("C:\Temp\SOR.xml")
            'do po query and count lines posted and return message to user
            'Return True
        Else
            ' MsgBox("Failed")
            Return "Failed"
        End If

    End Function
    Public Function getCustomer()


        Dim objSys As New SysproObj
        If objSys.QuerySyspro(CustomerXMl, "COMFND", "C:\Temp\customer.xml") Then
            Return objSys.FillCustomer("C:\Temp\customer.xml")
        End If
        ''insert detail lines



    End Function
    Public Function getWarehouse()

        Dim xml = WarehouseXMl()
        Dim objSys As New SysproObj
        If objSys.QuerySyspro(xml, "COMFND", "C:\Temp\warehouse.xml") Then
            Return objSys.FillWarehouse("C:\Temp\warehouse.xml")
        End If
        ''insert detail lines



    End Function

    Public Function PoHeader_xml(ByVal poHdr As PurchaseOrderMasterHdr) As String
        Dim xmlin As New StringBuilder

        With xmlin
            .Append("<PostPurchaseOrders xmlns:xsd=""http://www.w3.org/2001/XMLSchema-instance"" xsd:noNamespaceSchemaLocation=""PORTOIDOC.XSD"">")
            .Append("<Orders>")
            .Append("<OrderHeader>")
            .Append("<OrderActionType>A</OrderActionType>")
            .Append("<Supplier>" & poHdr.Supplier & "</Supplier>")
            .Append("<ExchRateFixed/>")
            .Append("<ExchangeRate/>")
            .Append("<OrderType>L</OrderType>")
            .Append("<Customer/>")
            .Append("<TaxStatus/>")
            .Append("<PaymentTerms/>")
            .Append("<InvoiceTerms/>")
            .Append("<CustomerPoNumber/>")
            .Append("<ShippingInstrs>" & poHdr.Ref & "</ShippingInstrs>")
            .Append("<OrderDate>" & Now.Date.ToString("yyyy-MM-dd") & "</OrderDate>")
            .Append("<DueDate>" & poHdr.DueDate.ToString("yyyy-MM-dd") & "</DueDate>")
            .Append("<MemoDate>" & Now.Date.ToString("yyyy-MM-dd") & "</MemoDate>")
            .Append("<ApplyDueDateToLines>A</ApplyDueDateToLines>")
            .Append("<MemoCode/>")
            .Append("<Buyer>" & poHdr.Buyer & "</Buyer>")
            .Append("<DeliveryName>" & poHdr.DeliveryAddress & "</DeliveryName>")
            .Append("<DeliveryAddr1>" & poHdr.DeliveryAddress1 & "</DeliveryAddr1>")
            .Append("<DeliveryAddr2>" & poHdr.DeliveryAddress2 & "</DeliveryAddr2>")
            .Append("<DeliveryAddr3>" & poHdr.DeliveryAddress3 & "</DeliveryAddr3>")
            '.Append("<DeliveryAddr4>This is the delivery address 4</DeliveryAddr4>")
            '.Append("<DeliveryAddr5>This is the delivery address 5</DeliveryAddr5>")
            .Append("<PostalCode>1401</PostalCode>")
            .Append("<Warehouse/>")
            .Append("<DiscountLessPlus/>")
            .Append("<DiscPercent1/>")
            .Append("<DiscPercent2/>")
            .Append("<DiscPercent3/>")
            .Append("<PurchaseOrder/>")
            .Append("<ChgPOStatToReadyToPrint/>")
            .Append("</OrderHeader>")
            .Append("<OrderDetails>")
        End With

        Return xmlin.ToString

    End Function
    Public Function WarehouseXMl() As String
        'Declaration
        Dim Document As New System.Text.StringBuilder

        'Building Document content
        With Document
            .Append("<?xml version=""1.0"" encoding=""Windows-1252""?>")
            .Append("<!-- Copyright 1994-2014 SYSPRO Ltd.-->")
            .Append("<!--")
            .Append("Sample XML for the Generic find Business Object")
            .Append("-->")
            .Append("<Query xmlns:xsd=""http://www.w3.org/2001/XMLSchema-instance"" xsd:noNamespaceSchemaLocation=""COMFND.XSD"">")
            .Append("<TableName>InvWhControl</TableName>")
            .Append("<ReturnRows>1000</ReturnRows>")
            .Append("<Columns>")
            .Append("<Column>Description</Column>")
            .Append("<Column>Warehouse</Column>")
            .Append("</Columns>")
            .Append("<Where>")
            .Append("<Expression>")
            .Append("<OpenBracket>(</OpenBracket>")
            .Append("<Column>Warehouse</Column>")
            .Append("<Condition>EQ</Condition>")
            .Append("<Value>X40</Value>")
            .Append("<CloseBracket>)</CloseBracket>")
            .Append("</Expression>")
            .Append("<Expression>")
            .Append("<AndOr>Or</AndOr>")
            .Append("<OpenBracket>(</OpenBracket>")
            .Append("<Column>Warehouse</Column>")
            .Append("<Condition>EQ</Condition>")
            .Append("<Value>40</Value>")
            .Append("<CloseBracket>)</CloseBracket>")
            .Append("</Expression>")
            .Append("	<Expression>")
            .Append("<AndOr>Or</AndOr>")
            .Append("<OpenBracket>(</OpenBracket>")
            .Append("<Column>Warehouse</Column>")
            .Append("<Condition>EQ</Condition>")
            .Append("<Value>34</Value>")
            .Append("<CloseBracket>)</CloseBracket>")
            .Append("</Expression>")
            .Append("</Where>")
            .Append("<OrderBy>")
            .Append("<Column>Warehouse</Column>")
            .Append("</OrderBy>")
            .Append("</Query>")
        End With


        Return Document.ToString()
    End Function
    Public Function CustomerXMl() As String
        'Declaration
        Dim Document As New System.Text.StringBuilder

        'Building Document content
        With Document
            .Append("<?xml version=""1.0"" encoding=""Windows-1252""?>")
            .Append("<!-- Copyright 1994-2014 SYSPRO Ltd.-->")
            .Append("<!--")
            .Append("Sample XML for the Generic find Business Object")
            .Append("-->")
            .Append("<Query xmlns:xsd=""http://www.w3.org/2001/XMLSchema-instance"" xsd:noNamespaceSchemaLocation=""COMFND.XSD"">")
            .Append("<TableName>ArCustomer</TableName>")
            .Append("<Columns>")
            .Append("<Column>Customer</Column>")
            .Append("<Column>Name</Column>")
            .Append("</Columns>")
            .Append("<OrderBy>")
            .Append("<Column>Customer</Column>")
            .Append("</OrderBy>")
            .Append("</Query>")
        End With
        Return Document.ToString
    End Function
    Public Function SOHeader_xml(ByVal poHdr As SOHdr) As String
        'Declaration
        'Declaration
        Dim Document As New System.Text.StringBuilder

        'Building Document content
        With Document
            .Append("<?xml version=""1.0"" encoding=""Windows-1252""?>")
            .Append("<!-- Copyright 1994-2010 SYSPRO Ltd.-->")
            .Append("<!--")
            .Append("This is an example XML instance to demonstrate")
            .Append("use of the Sales Order Transaction Posting Business Object")
            .Append("-->")
            .Append("<SalesOrders xmlns:xsd=""http://www.w3.org/2001/XMLSchema-instance"" xsd:noNamespaceSchemaLocation=""SORTOIDOC.XSD"">")
            .Append("<TransmissionHeader>")
            .Append("<TransmissionReference>00000000000003</TransmissionReference>")
            .Append("<SenderCode/>")
            .Append("<ReceiverCode>HO</ReceiverCode>")
            .Append("<DatePrepared>2006-11-04</DatePrepared>")
            .Append("<TimePrepared>11:55</TimePrepared>")
            .Append("</TransmissionHeader>")
            .Append("<Orders>")
            .Append("<OrderHeader>")
            .Append("<CustomerPoNumber>" + poHdr.PO + "</CustomerPoNumber>")
            .Append("<OrderActionType>A</OrderActionType>")
            .Append("<Customer>" + poHdr.CustomerCode + "</Customer>")
            .Append("<OrderDate>" + poHdr.OrderDate.ToString("yyyy-MM-dd") + "</OrderDate>")
            .Append("<InvoiceTerms>d</InvoiceTerms>")
            .Append("<Currency>$</Currency>")
            .Append("<ShippingInstrs>our freight, FOC</ShippingInstrs>")
            .Append("<ShippingInstrsCode/>")
            .Append("<CustomerName>" + poHdr.CustomerName.ToUpper() + "</CustomerName>")
            .Append("<ShipAddress1>" + poHdr.ShipPhone + "</ShipAddress1>")
            .Append("<ShipAddress2>" + poHdr.ShipAddr1 + "</ShipAddress2>")
            .Append("<ShipAddress3>" + poHdr.ShipAddr3 + "</ShipAddress3>")
            .Append("<ShipAddress3Locality></ShipAddress3Locality>")
            .Append("<ShipAddress4>" + poHdr.ShipAddr2 + "</ShipAddress4>")
            .Append("<ShipAddress5></ShipAddress5>")
            .Append("<ShipPostalCode>" + poHdr.ShipZip + "</ShipPostalCode>")
            .Append("<Warehouse>" + poHdr.Warehouse + "</Warehouse>")
            .Append("<SpecialInstrs>NO INVOICE WITH GOODS</SpecialInstrs>")
            .Append("<IgnoreWarnings>Y</IgnoreWarnings>")
            .Append("<SalesOrder></SalesOrder>")
            .Append("<OrderType></OrderType>")
            .Append("<MultiShipCode/>")
            .Append("<ShipAddressPerLine/>")
            .Append("<AlternateReference/>")
            .Append("<Salesperson>Sam</Salesperson>")
            .Append("<Branch/>")
            .Append("<Area/>")
            .Append("<RequestedShipDate>" + poHdr.ShipDate.ToString("yyyy-MM-dd") + "</RequestedShipDate>")
            .Append("<InvoiceNumberEntered/>")
            .Append("<InvoiceDateEntered/>")
            .Append("<OrderComments/>")
            .Append("<Nationality/>")
            .Append("<DeliveryTerms/>")
            .Append("<ShippingLocation></ShippingLocation>")
            .Append("<TransactionNature/>")
            .Append("<TransportMode/>")
            .Append("<ProcessFlag/>")
            .Append("<TaxExemptNumber/>")
            .Append("<TaxExemptionStatus/>")
            .Append("<GstExemptNumber/>")
            .Append("<GstExemptionStatus/>")
            .Append("<CompanyTaxNumber/>")
            .Append("<ShipAddressPerLineTax> </ShipAddressPerLineTax>")
            .Append("<CancelReasonCode/>")
            .Append("<DocumentFormat/>")
            .Append("<State/>")
            .Append("<CountyZip/>")
            .Append("<City/>")
            .Append("<DeliveryRouteAction>N</DeliveryRouteAction>")
            .Append("<DeliveryRoute></DeliveryRoute>")
            .Append("<InvoiceWholeOrderOnly/>")
            .Append("<IncludeInMrp>Y</IncludeInMrp>")
            .Append("<eSignature/>")
            .Append("</OrderHeader>")
            .Append("<OrderDetails>")
        End With

        Return Document.ToString()

    End Function
    Public Function SOStockLine_xml(ByVal poLine As SODtl) As String
        Dim Document As New System.Text.StringBuilder

        'Building Document content
        With Document
            .Append("<StockLine>")
            .Append("<CustomerPoLine>" + poLine.LineNumber.ToString() + "</CustomerPoLine>")
            .Append("<LineActionType>A</LineActionType>")
            .Append("<LineCancelCode/>")
            .Append("<StockCode>" + poLine.ItemCode + "</StockCode>")
            .Append("<StockDescription></StockDescription>")
            .Append("<Warehouse>" + poLine.Warehouse + "</Warehouse>")
            .Append("<CustomersPartNumber></CustomersPartNumber>")
            .Append("<OrderQty>" + poLine.Qty.ToString() + "</OrderQty>")
            .Append("<OrderUom>" & poLine.UOM & "</OrderUom>")
            .Append("<Price>" & poLine.UnitPrice.ToString.Replace(",", ".") & "</Price>")
            .Append("<PriceUom>" & poLine.UOM & "</PriceUom>")
            .Append("<PriceCode/>")
            .Append("<AlwaysUsePriceEntered/>")
            .Append("<Units/>")
            .Append("<Pieces/>")
            .Append("<ProductClass/>")
            .Append("<LineDiscPercent1>" + poLine.DiscPer.ToString() + "</LineDiscPercent1>")
            .Append("<LineDiscPercent2>0</LineDiscPercent2>")
            .Append("<LineDiscPercent3>0</LineDiscPercent3>")
            .Append("<AlwaysUseDiscountEntered>N</AlwaysUseDiscountEntered>")
            .Append("<CustRequestDate>" + poLine.shipdate.ToString("yyyy-MM-dd") + "</CustRequestDate>")
            .Append("<CommissionCode/>")
            .Append("<LineShipDate/>")
            .Append("<LineDiscValue>0</LineDiscValue>")
            .Append("<LineDiscValFlag/>")
            .Append("<OverrideCalculatedDiscount/>")
            .Append("<UserDefined></UserDefined>")
            .Append("<NonStockedLine/>")
            .Append("<NsUnitCost/>")
            .Append("<UnitMass/>")
            .Append("<UnitVolume/>")
            .Append("<StockTaxCode/>")
            .Append("<StockNotTaxable/>")
            .Append("<StockFstCode/>")
            .Append("<StockNotFstTaxable/>")
            .Append("<AllocationAction/>")
            .Append("<ConfigPrintInv/>")
            .Append("<ConfigPrintDel/>")
            .Append("<ConfigPrintAck/>")
            .Append("<TariffCode/>")
            .Append("<LineMultiShipCode/>")
            .Append("<SupplementaryUnitsFactor/>")
            .Append("<ReserveStock/>")
            .Append("<ReserveStockRequestAllocs/>")
            .Append("<IncludeInMrp>Y</IncludeInMrp>")
            .Append("<DeliveryLoad>")
            .Append("			<DeliveryRouteCode></DeliveryRouteCode>")
            .Append("			<DeliveryDate></DeliveryDate>")
            .Append("			<DeliverySequence></DeliverySequence>")
            .Append("</DeliveryLoad>")
            .Append("</StockLine>")
        End With
        Return Document.ToString()

    End Function

    Public Function PoStockLine_xml(ByVal poLine As PurchaseOrderDetails) As String
        Dim xmlin As New StringBuilder

        With xmlin
            .Append("<StockLine>")
            .Append("<PurchaseOrderLine>" & poLine.LineNumber.ToString() & "</PurchaseOrderLine>")
            .Append("<LineActionType>A</LineActionType>")
            .Append("<StockCode>" & poLine.ItemCode & "</StockCode>")
            .Append("<StockDescription/>")
            .Append("<Warehouse>" & poLine.warehouse & "</Warehouse>")
            .Append("<SupCatalogue>" & poLine.SupplierCat & "</SupCatalogue>")
            .Append("<OrderQty>" & poLine.Qty & "</OrderQty>")
            .Append("<OrderUom/>")
            .Append("<Units/>")
            .Append("<Pieces/>")
            .Append("<PriceMethod/>")
            .Append("<SupplierContract/>")
            .Append("<SupplierContractReference/>")
            .Append("<Price>" & poLine.UnitPrice & "</Price>")
            .Append("<PriceUom/>")
            .Append("<LineDiscType/>")
            .Append("<LineDiscLessPlus/>")
            .Append("<LineDiscPercent1/>")
            .Append("<LineDiscPercent2/>")
            .Append("<LineDiscPercent3/>")
            .Append("<LineDiscValue/>")
            .Append("<Taxable/>")
            .Append("<TaxCode/>")
            .Append("<Job/>")
            .Append("<HierHead></HierHead>")
            .Append("<Section1></Section1>")
            .Append("<Section2></Section2>")
            .Append("<Section3></Section3>")
            .Append("<Section4></Section4>")
            .Append("<Version/>")
            .Append("<Release/>")
            .Append("<LatestDueDate>" & poLine.ShipDate.ToString("yyyy-MM-dd") & "</LatestDueDate>")
            .Append("<OriginalDueDate/>")
            .Append("<RescheduleDueDate/>")
            .Append("<LedgerCode/>")
            .Append("<PasswordForLedgerCode/>")
            .Append("<SubcontractOp/>")
            .Append("<InspectionReqd/>")
            .Append("<ProductClass>" & poLine.ProductClass & "</ProductClass>")
            .Append("<NonsUnitMass/>")
            .Append("<NonsUnitVol/>")
            .Append("</StockLine>")
        End With

        Return xmlin.ToString

    End Function

    Public Function PoNonStockLine_xml(ByVal detail As PurchaseOrderDetails) As String
        Dim xmlin As New StringBuilder
        With xmlin
            .Append("<StockLine>")
            .Append("<PurchaseOrderLine>" & detail.LineNumber & "</PurchaseOrderLine>")
            .Append("<LineActionType>A</LineActionType>")
            .Append("<StockCode>" & detail.ItemCode & "</StockCode>")
            .Append("<StockDescription>" & detail.Description & "</StockDescription>")
            .Append("<Warehouse/>")
            .Append("<SupCatalogue>" & detail.SupplierCat & "</SupCatalogue>")
            .Append("<OrderQty>" & detail.Qty & "</OrderQty>")
            .Append("<OrderUom/>")
            .Append("<Units/>")
            .Append("<Pieces/>")
            .Append("<PriceMethod>M</PriceMethod>")
            .Append("<SupplierContract/>")
            .Append("<SupplierContractReference/>")
            .Append("<Price>" & detail.UnitPrice & "</Price>")
            .Append("<PriceUom/>")
            .Append("<LineDiscType>P</LineDiscType>")
            .Append("<LineDiscLessPlus>L</LineDiscLessPlus>")
            .Append("<LineDiscPercent1/>")
            .Append("<LineDiscPercent2/>")
            .Append("<LineDiscPercent3/>")
            .Append("<LineDiscValue/>")
            .Append("<Taxable>Y</Taxable>")
            .Append("<TaxCode/>")
            .Append("<Job/>")
            .Append("<HierHead></HierHead>")
            .Append("<Section1></Section1>")
            .Append("<Section2></Section2>")
            .Append("<Section3></Section3>")
            .Append("<Section4></Section4>")
            .Append("<Version/>")
            .Append("<Release/>")
            .Append("<LatestDueDate>" & detail.ShipDate.ToString("yyyy-MM-dd") & "</LatestDueDate>")
            .Append("<OriginalDueDate/>")
            .Append("<RescheduleDueDate/>")
            If detail.glCode IsNot Nothing Then
                .Append("<LedgerCode>" & detail.glCode & "</LedgerCode>")
            Else
                .Append("<LedgerCode/>")
            End If
            .Append("<PasswordForLedgerCode/>")
            .Append("<SubcontractOp/>")
            .Append("<InspectionReqd/>")
            .Append("<ProductClass>" & detail.ProductClass & "</ProductClass>")
            .Append("<NonsUnitMass/>")
            .Append("<NonsUnitVol/>")
            .Append("<NonStockedLine>Y</NonStockedLine>")
            .Append("</StockLine>")
        End With
        Return xmlin.ToString
    End Function

    Public Function PoCommentline_xml(ByVal poComm As PurchaseOrderCommentLine) As String
        Dim xmlin As New StringBuilder

        With xmlin
            .Append("<CommentLine>")
            .Append("<PurchaseOrderLine>" & poComm.LineNum & "</PurchaseOrderLine>")
            .Append("<LineActionType>A</LineActionType>")
            .Append("<Comment>" & poComm.Comment & "</Comment>")
            .Append("<AttachedToStkLineNumber>" & poComm.LineNum - 1 & "</AttachedToStkLineNumber>")
            .Append("<DeleteAttachedCommentLines/>")
            .Append("<ChangeSingleCommentLine/>")
            .Append("</CommentLine>")
        End With
        Return xmlin.ToString

    End Function

    Public Function PoMisc_xml() As String
        Dim xmlin As New StringBuilder

        With xmlin
            .Append("<MiscChargeLine>")
            .Append("<PurchaseOrderLine>3</PurchaseOrderLine>")
            .Append("<LineActionType>A</LineActionType>")
            .Append("<MiscChargeValue/>")
            .Append("<MiscChargeTax>A</MiscChargeTax>")
            .Append("<MiscTaxable>Y</MiscTaxable>")
            .Append("<MiscChargeFLoc>L</MiscChargeFLoc>")
            .Append("<MiscDescription>Sundry Items</MiscDescription>")
            .Append("<MiscCommentCode/>")
            .Append("</MiscChargeLine>")
        End With

        Return xmlin.ToString

    End Function

    Public Function PoFreight_chargeLine_xml(ByVal intLine As Integer) As String
        Dim xmlin As New StringBuilder

        With xmlin
            .Append("<FreightLine>")
            .Append("<PurchaseOrderLine>" & intLine & "</PurchaseOrderLine>")
            .Append("<LineActionType>A</LineActionType>")
            .Append("<FreightValue>0.00</FreightValue>")
            .Append("<FreightTaxCode>A</FreightTaxCode>")
            .Append("<FreightTaxable>Y</FreightTaxable>")
            .Append("<FreightFLoc>L</FreightFLoc>")
            .Append("</FreightLine>")

        End With

        Return xmlin.ToString

    End Function

    Public Function PO_Footer_xml() As String
        Dim xmlin As New StringBuilder

        With xmlin
            .Append("</OrderDetails>")
            .Append("</Orders>")
            .Append("</PostPurchaseOrders>")
        End With

        Return xmlin.ToString

    End Function
    Public Function SO_Footer_xml() As String
        Dim xmlin As New StringBuilder

        With xmlin

            .Append("</OrderDetails>")
            .Append("</Orders>")
            .Append("</SalesOrders>")
        End With

        Return xmlin.ToString

    End Function

    Public Function SoParam_xml() As String
        'Declaration
        Dim Parameter As New System.Text.StringBuilder

        'Building Parameter content
        With Parameter
            .Append("<?xml version=""1.0"" encoding=""Windows-1252""?>")
            .Append("<!-- Copyright 1994-2010 SYSPRO Ltd.-->")
            .Append("<!--")
            .Append("This is an example XML instance to demonstrate")
            .Append("use of the Sales Order Transaction Posting Business Object")
            .Append("-->")
            .Append("<SalesOrders xmlns:xsd=""http://www.w3.org/2001/XMLSchema-instance"" xsd:noNamespaceSchemaLocation=""SORTOI.XSD"">")
            .Append("<Parameters>")
            .Append("<InBoxMsgReqd>N</InBoxMsgReqd>")
            .Append("<Process>IMPORT</Process>")
            .Append("<CustomerToUse/>")
            .Append("<WarehouseListToUse>34</WarehouseListToUse>")
            .Append("<UseCustomerSalesWarehouse></UseCustomerSalesWarehouse>")
            .Append("<TypeOfOrder>SOH</TypeOfOrder>")
            .Append("<OrderStatus>1</OrderStatus>")
            .Append("<MinimumDaysToShip>0</MinimumDaysToShip>")
            .Append("<AllowNonStockItems>Y</AllowNonStockItems>")
            .Append("<AcceptOrdersIfNoCredit>Y</AcceptOrdersIfNoCredit>")
            .Append("<AcceptEarlierShipDate>Y</AcceptEarlierShipDate>")
            .Append("<OperatorToInform>ADMIN</OperatorToInform>")
            .Append("<CreditFailMessage>No credit available</CreditFailMessage>")
            .Append("<ValidProductClassList/>")
            .Append("<ShipFromDefaultBin>N</ShipFromDefaultBin>")
            .Append("<AllowDuplicateOrderNumbers>Y</AllowDuplicateOrderNumbers>")
            .Append("<CheckForCustomerPoNumbers>Y</CheckForCustomerPoNumbers>")
            .Append("<AllowInvoiceInformationEntry/>")
            .Append("<AlwaysUsePriceEntered/>")
            .Append("<AllowZeroPrice/>")
            .Append("<AllowChangeToZeroPrice></AllowChangeToZeroPrice>")
            .Append("<AddStockSalesOrderText>N</AddStockSalesOrderText>")
            .Append("<AddDangerousGoodsText>N</AddDangerousGoodsText>")
            .Append("<UseStockDescSupplied/>")
            .Append("<ValidateShippingInstrs/>")
            .Append("<AllocationAction></AllocationAction>")
            .Append("<IgnoreWarnings>Y</IgnoreWarnings>")
            .Append("<IgnoreErrors>Y</IgnoreErrors>")
            .Append("<AddAttachedServiceCharges></AddAttachedServiceCharges>")
            .Append("<StatusInProcess></StatusInProcess>")
            .Append("<StatusInProcessResponse></StatusInProcessResponse>")
            .Append("<WarnIfCustomerOnHold>N</WarnIfCustomerOnHold>")
            .Append("<AcceptKitOptional>N</AcceptKitOptional>")
            .Append("<AllowBackOrderForPartialHold></AllowBackOrderForPartialHold>")
            .Append("<AllowBackOrderForSuperseded></AllowBackOrderForSuperseded>")
            .Append("<OverrideCustomerBackOrder></OverrideCustomerBackOrder>")
            .Append("<UseMasterAccountForCustomerPartNo></UseMasterAccountForCustomerPartNo>")
            .Append("<ApplyLeadTimeCalculation></ApplyLeadTimeCalculation>")
            .Append("<ApplyParentDiscountToComponents>N</ApplyParentDiscountToComponents>")
            .Append("<AllowManualOrderNumberToBeUsed>N</AllowManualOrderNumberToBeUsed>")
            .Append("<ReserveStock></ReserveStock>")
            .Append("<ReserveStockRequestAllocs></ReserveStockRequestAllocs>")
            .Append("<AllowBackOrderForNegativeMerchLine>N</AllowBackOrderForNegativeMerchLine>")
            .Append("<PutEntireQuantityOnNewLoadWhenChanged>N</PutEntireQuantityOnNewLoadWhenChanged>")
            .Append("<MinimumChargeLineZero>N</MinimumChargeLineZero>")
            .Append("<UpdateWhLast>N</UpdateWhLast>")
            .Append("</Parameters>")
            .Append("</SalesOrders>")
        End With


        Return Parameter.ToString()


    End Function

    Public Function SysproPost(ByVal xmlin As StringBuilder, ByVal xmlParam As StringBuilder)

        Dim objSys As New SysproObj

        If objSys.postSysproXML(xmlin.ToString, xmlParam.ToString, "PORTOI", "C:\Temp\POR.xml") Then
            objSys.Read_Xml("C:\Temp\POR.xml")
            Return True
        Else
            Return False
        End If

    End Function

    Public Function PorQry_xml(ByVal Por As String) As String
        Dim XMLout As String = Nothing
        'build an xml string to pass parameters
        Dim XMLin As New System.Text.StringBuilder
        Dim SysObj As New SysproObj
        If SysObj.SysproConnect = True Then

            'Building Document content
            With XMLin
                .Append("<Query xmlns:xsd=""http://www.w3.org/2001/XMLSchema-instance"" xsd:noNamespaceSchemaLocation=""PORQRY.XSD"">")
                .Append("<Key>")
                .Append("<PurchaseOrder>" & Por & "</PurchaseOrder>")
                .Append("</Key>")
                .Append("<Option>")
                .Append("<IncludeStockedLines>Y</IncludeStockedLines>")
                .Append("<IncludeNonStockedLines>Y</IncludeNonStockedLines>")
                .Append("<IncludeFreightLines>Y</IncludeFreightLines>")
                .Append("<IncludeMiscellaneousLines>Y</IncludeMiscellaneousLines>")
                .Append("<IncludeCommentLines>Y</IncludeCommentLines>")
                .Append("<IncludeCompletedLines>Y</IncludeCompletedLines>")
                .Append("<IncludeGrns>Y</IncludeGrns>")
                .Append("<IncludeHistory>Y</IncludeHistory>")
                .Append("<IncludeLctDetails>Y</IncludeLctDetails>")
                .Append("<IncludeRequisitionDetails>Y</IncludeRequisitionDetails>")
                .Append("<IncludeRequisitionRouting>Y</IncludeRequisitionRouting>")
                .Append("<IncludeSalesOrders>Y</IncludeSalesOrders>")
                .Append("<IncludeCustomForms>N</IncludeCustomForms>")
                .Append("<XslStylesheet/>")
                .Append("</Option>")
                .Append("</Query>")
            End With
            Try
                XMLout = SysObj.Qry.Query(SysObj.UserID, "PORQRY", XMLin.ToString())
            Catch exc As Exception
                MsgBox(exc.Message, , "Purchase Order Query")
            End Try

            If Not (XMLout Is Nothing) Then
                Dim XMLfile As Boolean = SaveTextToFile(XMLout, "C:\Temp\PORQRY.xml", "")

                'logoff  syspr session
                SysObj.LogOffSyspro()
                SysObj.Qry = Nothing
                SysObj.Util = Nothing
                Return True
            Else
                Return False
            End If
        Else
            Return False
        End If

    End Function

    Public Function GetPorLines(ByVal poNum As Integer) As Array
        Dim porObj As New PurchaseOrderBuild
        Dim i As Integer = -1
        GetPorLines = Nothing

        If porObj.PorQry_xml(poNum) Then
            'read xml to get por lines and requisition line number in catalogue

            Dim document As New XmlDocument

            document.Load("C:\Temp\PORQRY.xml")

            'get number of order details
            For Each element As XmlElement In document.SelectNodes("PorDetail/PurchaseOrderLine")
                i += 1
            Next
            Dim arr(i, 1) As String

            'store syspro sales line number and Etrack line number to array
            For Each element As XmlElement In document.SelectNodes("PorDetail/PurchaseOrderLine/Merchandise")
                arr(i, 0) = (element.SelectSingleNode("Line").InnerText) 'Line Number
                arr(i, 1) = (element.SelectSingleNode("MSupCatalogue").InnerText) 'etrack Requisition line number
                i -= 1
            Next

            Return arr
        Else
            Return Nothing
        End If

    End Function

    'Public Function Read_Xml(ByVal xml_Path As String) As String

    '    Dim tr As XmlTextReader = New XmlTextReader(xml_Path)
    '    Dim doc As New XmlDocument
    '    Dim POnum As String = Nothing
    '    Dim PoLine As String = Nothing
    '    Dim GrnNumber As String = Nothing
    '    doc.Load(xml_Path)
    '    tr.Read()
    '    While tr.Read
    '        If tr.NodeType.Equals(XmlNodeType.Element) AndAlso tr.Name.Equals("ErrorDescription") AndAlso tr.IsStartElement() Then
    '            Dim strXMLErr As String = tr.ReadString

    '            Return MsgBox("SYSPRO MESSAGE: " & vbNewLine & vbNewLine & _
    '                    strXMLErr, MsgBoxStyle.Critical).ToString
    '        End If
    '    End While

    '    If xml_Path = "C:\Temp\POR.xml" Then
    '        Try
    '            For Each po As XmlElement In doc.SelectNodes("postpurchaseorders/Order/Key")
    '                POnum = po.SelectSingleNode("PurchaseOrder").InnerText
    '            Next
    '            Return MsgBox("Done Posting:" & vbNewLine & vbNewLine & POnum, MsgBoxStyle.Information, "Created PO")
    '        Catch ex As Exception
    '            MsgBox(ex.Message)
    '        End Try

    '    ElseIf xml_Path = "C:\Temp\PorReceipts.xml" Then
    '        Try
    '            For Each po As XmlElement In doc.SelectNodes("postpurchaseorderreceipts/Item/Receipt")
    '                POnum = po.SelectSingleNode("PurchaseOrder").InnerText
    '                GrnNumber = po.SelectSingleNode("Grn").InnerText
    '                PoLine = po.SelectSingleNode("PurchaseOrderLine").InnerText
    '            Next
    '            Return MsgBox("Done Posting:" & vbNewLine & vbNewLine & _
    '                                              "PO: " & POnum & vbNewLine & vbNewLine & _
    '                                             "PO Line: " & PoLine & vbNewLine & vbNewLine & _
    '                                              "Grn Created: " & GrnNumber, MsgBoxStyle.Information, "POR Receipt")
    '        Catch ex As Exception
    '            MsgBox(ex.Message)
    '        End Try

    '    End If

    '    Return MsgBox("Done Posting:", MsgBoxStyle.Information)

    'End Function

End Class

Public Class DeliveryInfo
    Public Property DeliveryName
    Public Property Delivery1
    Public Property Delivery2
    Public Property Delivery3
    Public Property Delivery4
End Class


