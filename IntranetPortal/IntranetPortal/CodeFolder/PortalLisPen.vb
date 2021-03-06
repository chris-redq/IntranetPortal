﻿Partial Public Class PortalLisPen
    Public ReadOnly Property Type As String
        Get
            If Not String.IsNullOrEmpty(LpType) Then
                Return LpType
            End If

            Return "Foreclosure"
        End Get
    End Property

    Public ReadOnly Property Effective As Date
        Get
            Return FileDate
        End Get
    End Property

    'Public ReadOnly Property Expiration As Object
    '    Get
    '        Return ""
    '    End Get
    'End Property

    Public ReadOnly Property Index As String
        Get
            If Not String.IsNullOrEmpty(Docket_Number) AndAlso FileDate.Year > 0 Then
                If Docket_Number.Contains("/") Or Docket_Number.Contains("-") Then
                    Return Docket_Number
                End If

                Return String.Format("{0}/{1}", Docket_Number.TrimEnd, FileDate.Year)
            Else
                Return Docket_Number
            End If
        End Get
    End Property

End Class
