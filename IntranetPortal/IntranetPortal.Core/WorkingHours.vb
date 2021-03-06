﻿Public Class WorkingHours

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="startDate"></param>
    ''' <param name="endDate"></param>
    ''' <returns>
    ''' for day:   5.00.00.00
    ''' for hours: 2:00:00
    ''' </returns>
    ''' <remarks></remarks>
    Public Shared Function GetWorkingDays(startDate As DateTime, endDate As DateTime, emp As String) As TimeSpan
        Dim dayCount = 0

        If startDate.Date = endDate.Date Then
            Return endDate - startDate
        End If

        While startDate.Date < endDate.Date
            If startDate.DayOfWeek <> DayOfWeek.Sunday AndAlso startDate.DayOfWeek <> DayOfWeek.Saturday AndAlso Not IsHoliday(startDate, emp) Then
                dayCount += 1
            End If

            startDate = startDate.AddDays(1)
        End While

        Return (endDate - startDate).Add(TimeSpan.Parse(dayCount & ".00:00:00"))
    End Function

    Public Shared Function AddWorkDays(startDate As DateTime, days As Integer) As DateTime
        Dim returnDate = startDate
        Dim count = 0

        Dim steps = 1
        If days < 0 Then
            steps = -1
        End If

        While Math.Abs(count) < Math.Abs(days)
            returnDate = returnDate.AddDays(steps)
            If IsWorkingDay(returnDate) Then
                count = count + steps
            End If
        End While

        Return returnDate
    End Function

    Private Shared Function IsHoliday(startDate As DateTime, emp As String) As Boolean
        Return IntranetPortal.Core.HolidayService.IsHoliday(startDate.Date, emp)
    End Function

    Public Shared Function IsWorkingDay(dt As DateTime) As Boolean
        If dt.DayOfWeek = DayOfWeek.Sunday Or dt.DayOfWeek = DayOfWeek.Saturday Then
            Return False
        End If

        If IntranetPortal.Core.HolidayService.IsPublicHoliday(dt) Then
            Return False
        End If

        Return True
    End Function

    Public Shared Function IsWorkingHour(dt As DateTime) As Boolean
        If Not IsWorkingDay(dt) Then
            Return False
        End If

        If dt.Hour >= 18 Or dt.Hour < 9 Then
            Return False
        End If

        Return True
    End Function
End Class