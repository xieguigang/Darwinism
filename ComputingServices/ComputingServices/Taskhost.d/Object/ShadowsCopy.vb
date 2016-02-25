Imports System.Reflection
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel

Namespace TaskHost

    Module ShadowsCopy

        ''' <summary>
        ''' 将客户端上面的对象数据复制到远程主机上面的内存管理模块之中
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
        ''' <param name="target">远程服务器上面的对象</param>
        ''' <param name="memory">远程对象的内存管理模块</param>
        ''' <param name="stack">Avoided of the loop reference.(内存管理的复制堆栈记录)</param>
        ''' <returns></returns>
        Private Function __innerCopy(from As Object, target As Object, memory As MemoryHash, stack As List(Of Long)) As Boolean
            Dim props As PropertyInfo() = from.GetType.GetReadWriteProperties

            For Each prop As PropertyInfo In props
                Dim value As Object = prop.GetValue(from)

                If BasicTypesFlushs.ContainsKey(prop.PropertyType) Then
                    Call prop.SetValue(target, value)  ' 值类型，直接复制
                Else   ' 引用类型，递归按址复制
                    Dim addr As ObjectAddress = ObjectAddress.AddressOf(value) ' 得到对象在内存之中的位置指针
                    If memory.IsNull(addr.ReferenceAddress) Then
                        Call memory.SetObject(value)  ' 空的，则直接插入
                    Else
                        Dim innerTarget As Object = prop.GetValue(target)

                        addr = ObjectAddress.AddressOf(innerTarget)  ' 假若是引用类型的对象，在复制的时候还需要检查栈空间，否则会出现死循环 栈空间溢出

                        ' 检查栈空间是否已经复制过当前的对象了？
                        If stack.IndexOf(addr.ReferenceAddress) = -1 Then     ' 假若出现循环引用的话，应该怎样进行复制？？
                            Call stack.Add(addr.ReferenceAddress)      ' 写栈路径记录
                            Call __innerCopy(value, innerTarget, memory, stack)  ' 假若已经存在了，则递归进入下一层复制
                        End If
                    End If
                End If
            Next

            Return True
        End Function
    End Module
End Namespace