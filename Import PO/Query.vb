Imports System.Xml
Imports System.Text
Public Class Query

    Public Function QryStockCode_xml(ByVal stockcode As String) As String
        'Declaration
        Dim Document As New StringBuilder

        'Building Document content
        With Document

            .Append("<Query xmlns:xsd=""http://www.w3.org/2000/10/XMLSchema-instance"" xsd:noNamespaceSchemaLocation=""INVQRY.XSD"">")
            .Append("<Key>")
            .Append("<StockCode>" & stockcode & "</StockCode>")
            .Append("</Key>")
            .Append("<Option>")
            .Append("<MultiMediaImageType>GIF</MultiMediaImageType>")
            .Append("<IncludeHistory>N</IncludeHistory>")
            .Append("<IncludeBins>Y</IncludeBins>")
            .Append("<IncludeLots>Y</IncludeLots>")
            .Append("<IncludeSerials>Y</IncludeSerials>")
            .Append("<IncludeMovements>N</IncludeMovements>")
            .Append("<MovementDateSequence>A</MovementDateSequence>")
            .Append("<MovementStartDate>2001-01-12</MovementStartDate>")
            .Append("<MaxNumberMovements>1</MaxNumberMovements>")
            .Append("<IncludeCustomForms>N</IncludeCustomForms>")
            .Append("<IncludeMovementIssues>N</IncludeMovementIssues>")
            .Append("<IncludeMovementTransfers>N</IncludeMovementTransfers>")
            .Append("<IncludeMovementReceipts>N</IncludeMovementReceipts>")
            .Append("<IncludeMovementPhysical>N</IncludeMovementPhysical>")
            .Append("<IncludeMovementAdjustments>N</IncludeMovementAdjustments>")
            .Append("<IncludeMovementCostChanges>N</IncludeMovementCostChanges>")
            .Append("<IncludeMovementCostMods>N</IncludeMovementCostMods>")
            .Append("<IncludeMovementBinTransfers>N</IncludeMovementBinTransfers>")
            .Append("<IncludeMovementInvoices>N</IncludeMovementInvoices>")
            .Append("<IncludeMovementCreditNotes>N</IncludeMovementCreditNotes>")
            .Append("<IncludeMovementDebitNotes>N</IncludeMovementDebitNotes>")
            .Append("<IncludeMovementDispatchNotes>N</IncludeMovementDispatchNotes>")
            .Append("<IncludeEcc>Y</IncludeEcc>")
            .Append("<IncludeApprovedManuf>Y</IncludeApprovedManuf>")
            .Append("<IncludeNarrations>Y</IncludeNarrations>")
            .Append("<ReturnNarrationsinBlock>N</ReturnNarrationsinBlock>")
            .Append("<XslStylesheet/>")
            .Append("</Option>")
            .Append("<Filter>")
            .Append("<Warehouse FilterType=""L"" FilterValue=""S,N,E""/>")
            .Append("</Filter>")
            .Append("</Query>")

        End With
        Return Document.ToString
    End Function

    Public Function QrySupplier_xml(ByVal supplier As String) As String
        'Declaration
        Dim Document As New System.Text.StringBuilder

        'Building Document content
        With Document
            .Append("<Query xmlns:xsd=""http://www.w3.org/2001/XMLSchema-instance"" xsd:noNamespaceSchemaLocation=""APSQRY.XSD"">")
            .Append("<Key>")
            .Append("<Supplier>" & supplier & "</Supplier>")
            .Append("</Key>")
            .Append("<Option>")
            .Append("<IncludeInvoices>N</IncludeInvoices>")
            .Append("<IncludeReceipts>N</IncludeReceipts>")
            .Append("<IncludeScrapReject>N</IncludeScrapReject>")
            .Append("<IncludeUnapprovedInvInCashProj>N</IncludeUnapprovedInvInCashProj>")
            .Append("<CashProjectionAtCurrentRate>N</CashProjectionAtCurrentRate>")
            .Append("<IncludeRetainedInvoices>N</IncludeRetainedInvoices>")
            .Append("<IncludeInvoicePayments>N</IncludeInvoicePayments>")
            .Append("<IncludeRegisteredInvoices>N</IncludeRegisteredInvoices>")
            .Append("<IncludeGoodsReceivedNotes>N</IncludeGoodsReceivedNotes>")
            .Append("<IncludeCustomForms>N</IncludeCustomForms>")
            .Append("<MultiMediaImageType>BMP</MultiMediaImageType>")
            .Append("<XslStylesheet/>")
            .Append("</Option>")
            .Append("</Query>")

        End With

        Return Document.ToString
    End Function

End Class
