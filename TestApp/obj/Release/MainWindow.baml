   M S B A M L   `   `   `  ����   LogFileManipulator#    LogFileManipulator.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml       � Icon$  ./docIcon.ico�� � Title$ Log File Manipulator��$	��620��$	��760��$	��380��$	��460��.��]� -   $��Base_Dock_Panel����. .+�]� $��Bottom=�$��Stretch=�$��5q���.  .+��� -   $��Import_Button��$��Import��$��5q�$
��Left=�$	��Top=�$��75��{�.   � ToolTip$ Feature not finished�� ^� ShowOnDisabled . �� -   $��Export_Button��$��Export��$��5q�$
��Left=�$	��Top=�$��75��{�.  $ Feature not finished�� . �� -   $��Cancel_Button��$��Cancel��$��5q�$��Right=�$	��Top=�$��75���� -   $��Save_All_Button��$��Save All��$��5q�$��Right=�$	��Top=�$��75��{�.  �� -   $��Save_Button��$
��Save��$��5q�$��Right=�$	��Top=�$��75��{�.  �� $��Bottom=�$��1q����� .��
Operations.��]� ��. .+�]� -   $��Add_Rows��$��Bottom=���.  $��Stretch=�.+��� -   $��About Regular Expressions��
����  �$��Right=��� -	   $��+ Add More Rows��$��5q�$��Center=��� -
   $��OperationsStack�� �� Orientation$ Vertical=��  � ColumnDefinitions �� -    �Name$ Left_Column��$	��100���� -   $ Right_Column��$	��100�� � RowDefinitions �� $
��auto ��� $��1* �.+�]� -   $��InputSelectionGrid��$��0��$��0����. $��5q�.+��� -   $��InputFileSelectionButton��$	��...��$��20��$��Right=��� -   $��InputClearButton��$��X��$��20��$��Right=�$y�Hidden=��� -   $��InputFileNameTextBlock��$��Center=�]� $��1��$��0����. $��1,5,0,5q�.+��� $��Center=�$
��Left=�.��Output File Extension:�� -   $��OutputFileName��$��Center=�{�. �� $��0��$��1��$
��Auto=�.���� -   $��InputFilePreview�� �� 
IsReadOnly . $	��100���� $��1��$��1��$
��Auto=�.���� -   $��OutputFilePreview�� . $	��100���� -   $��1��$��1��$
��Left=�$��2��
���   �