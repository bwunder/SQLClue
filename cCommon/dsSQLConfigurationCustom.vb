Namespace dsSQLConfigurationTableAdapters
    Partial Class tScheduleTableAdapter
        Public Overloads Function Update(ByVal ScheduleRow As DataRow, ByRef Id As Int32) As Integer
            ' ByRef is the key here, allows the value to go back to the caller
            Dim rc As Integer = Update(ScheduleRow)
            Id = CInt(Me._adapter.InsertCommand.Parameters("@Id").Value)
            Id = CInt(Me._adapter.UpdateCommand.Parameters("@Id").Value)
            Return rc
        End Function
    End Class
End Namespace


