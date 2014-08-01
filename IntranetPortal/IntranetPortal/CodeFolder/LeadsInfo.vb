﻿Public Class LeadsInfo
    Public Property HomeOwners As List(Of HomeOwner)

    Public ReadOnly Property LeadsName As String
        Get
            Return Utility.GetLeadsName(Me)
        End Get
    End Property

    Public ReadOnly Property Status As String
        Get
            If Lead.Status.HasValue Then
                Return CType(Lead.Status.Value, LeadStatus).ToString
            End If

            'Dim ld = IntranetPortal.Lead.GetInstance(BBLE)
            'If ld IsNot Nothing AndAlso ld.Status.HasValue Then
            '    Return CType(ld.Status, LeadStatus).ToString
            'End If

            Return ""
        End Get
    End Property

    Public ReadOnly Property HasOwnerInfo As Boolean
        Get
            Using context As New Entities
                Return context.HomeOwners.Where(Function(ho) ho.BBLE = BBLE And ho.Active = True And ho.LocateReport IsNot Nothing).Count > 0
            End Using
        End Get
    End Property


    Public ReadOnly Property Neighborhood As String
        Get
            'Dim neighbor = ""
            ''If Not String.IsNullOrEmpty(PropertyAddress) Then
            ''    If PropertyAddress.Split(",").Count > 1 Then
            ''        neighbor = PropertyAddress.Split(",")(1)
            ''        Return neighbor
            ''    End If
            ''End If

            Return NeighName
        End Get
    End Property

    Public ReadOnly Property LastIssuedOn As String
        Get
            If CreateDate.HasValue Then
                Return String.Format("Last Issued On: {0:g}", CreateDate)
            Else
                Return ""
            End If
        End Get
    End Property

    Public ReadOnly Property IndicatorOfLiens As String
        Get
            If Me.C1stMotgrAmt = 0 Then
                Return ""
            End If

            Return "<span style=""color:red"">Liens higher than Value</span>"
        End Get
    End Property

    Public ReadOnly Property IndicatorOfWater As String
        Get
            If Me.WaterAmt = 0 Then
                Return ""
            End If

            Return "<span style=""color:red"">High Water Indicator</span>"
        End Get
    End Property

    Public ReadOnly Property UpdateInfo As String
        Get
            Using context As New Entities

                If context.APIOrders.Where(Function(order) order.BBLE = BBLE And order.Status <> APIOrder.OrderStatus.Complete).Count > 0 Then
                    Return "<span style=""color:red"">Lead is waiting to update. It will complete in few minuties.</span>"
                Else
                    If LastUpdate Is Nothing Then
                        Return ""
                    Else
                        Return String.Format("Last Update: {0:g}", Me.LastUpdate.Value)
                    End If
                End If
            End Using
        End Get
    End Property

    Public Shared Function GetInstance(bble As String) As LeadsInfo
        Using context As New Entities
            Return context.LeadsInfoes.Where(Function(b) b.BBLE = bble).SingleOrDefault
        End Using
    End Function

    Public ReadOnly Property Violation As String
        Get
            If ECBViolationsAmt.HasValue AndAlso ECBViolationsAmt.Value > 0 Then
                Return "ECB"
            End If

            If DOBViolationsAmt.HasValue AndAlso DOBViolationsAmt.Value > 0 Then
                Return "DOB"
            End If

            Return ""
        End Get
    End Property

    Public ReadOnly Property ViolationAmount As Double
        Get
            If ECBViolationsAmt.HasValue AndAlso ECBViolationsAmt.Value > 0 Then
                Return ECBViolationsAmt.Value
            End If

            If DOBViolationsAmt.HasValue AndAlso DOBViolationsAmt.Value > 0 Then
                Return DOBViolationsAmt.Value
            End If

            Return Nothing
        End Get
    End Property
End Class
