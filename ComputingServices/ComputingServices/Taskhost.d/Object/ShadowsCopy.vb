Imports System.Reflection
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel

Namespace TaskHost

    Module ShadowsCopy

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="from">Client上面的</param>
        ''' <param name="target">服务器上面的</param>
        ''' <param name="memory">内存管理模块单元</param>
        ''' <returns></returns>
        Public Function ShadowsCopy(from As Object, target As Object, memory As MemoryHash) As Boolean
            Return __innerCopy(from, target, memory, New List(Of Long) From {ObjectAddress.AddressOf(target).ReferenceAddress})
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="from"></param>
        ''' <param name="target"></param>
        ''' <param name="memory"></param>
        ''' <param name="stack">Avoided of the loop reference</param>
        ''' <returns></returns>
        Private Function __innerCopy(from As Object, target As Object, memory As MemoryHash, stack As List(Of Long)) As Boolean
            Dim props As PropertyInfo() = from.GetType.GetReadWriteProperties

            For Each prop As PropertyInfo In props
                Dim value As Object = prop.GetValue(from)

                If BasicTypesFlushs.ContainsKey(prop.PropertyType) Then
                    Call prop.SetValue(target, value)  ' 值类型，直接复制
                Else   ' 引用类型，递归按址复制
                    Dim addr = ObjectAddress.AddressOf(value)
                    If memory.IsNull(addr.ReferenceAddress) Then
                        ' 空的，则直接插入
                        Call memory.SetObject(value)
                    Else
                        Dim innerTarget As Object = prop.GetValue(target)

                        addr = ObjectAddress.AddressOf(innerTarget)

                        ' 检查栈空间是否已经复制过当前的对象了？
                        If stack.IndexOf(addr.ReferenceAddress) = -1 Then     ' 假若出现循环引用的话，应该怎样进行复制？？
                            Call ShadowsCopy(value, innerTarget, memory)  ' 假若已经存在了，则递归进入下一层复制
                        End If
                    End If
                End If
            Next

            Return True
        End Function
    End Module
End Namespace