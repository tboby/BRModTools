   M S B A M L   `   `   `  ����E  ABRModTools, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:BRModTools
BRModTools  5      W SHelixToolkit, Version=2011.10.1.0, Culture=neutral, PublicKeyToken=52aa3500039caf0dA0clr-namespace:HelixToolkit;assembly=HelixToolkitHelixToolkit 5          BRModTools.UserControl1   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      *localclr-namespace:BRModTools     5      Bhelix0clr-namespace:HelixToolkit;assembly=HelixToolkit     5      -   5          >�Name$  UserControl��5      $	��300��5      $	��300��6    � 	Resources 5	         CameraTransform �� Rotation �� Axis �� Angle �� ElementName �� Path 
 Camera � Position � LookDirection	 � UpDirection
 =� FieldOfView �� 	Transform   BRModTools.BrushConverter  imgBrush    BRModTools.BrushConverterI  	imgBrushI��%�  &	        &	 �     &	 h    &	 {    Y� 5
   
   .�5      ��  5      �� 5      $ 1,0,05�6*    5      �� $
 sliX��$ Value�5      �� 5       5      �� 5      $ 0,1,05�6*    5      �� $
 sliY��$ Value�5      5      =� 5   
   $ 0, 0, 81�5      $ 0, 0, -85�5      $	 0, 1, 05�5      $
 30Q�5      # �   5        5   
   6     5   
   6    5       ��5$      �� 5%   
    �� 
StartPoint$	 0,0&�6    �� EndPoint$	 0,1&�6/   .��5&      �  � Color$ Yellow��6    � Offset$ 0Q�6*   � 5'      $	 Red��6   $ 1Q�6'   5(      .��5+      "  HelixToolkit.HelixViewport3D     ModelUpDirection$ 0,1,05�6      Children. 5,      g�  g� Content 5-      h� 5.      .��50      ��  �� Color$
 Gray��6'   t� 51      $
 Gray��6+    t� 	Direction$ -1,-3,-25�68   t� 52      $
 Gray��6+   $ 1,-2,35�68   h� 54      .��56      �  � Geometry 57   "   k� 58   &    k� 	Positions �    k� TriangleIndices �      k� TextureCoordinates �   5;       � Material 5=   "   u� 5>   &    u� Brush 66   �� *
faces[1]+ �� 	Converter# [ 5@   '   5B      � 5E       5F   "   k� 5G   &    �    �      �   5J       5L   "   u� 5M   &    66   �� *
faces[0]+# [ 5N   '   5P      � 5S       5T   "   k� 5U   &    �    �      �   5X       5Z   "   u� 5[   &    66   �� *
faces[2]+# [ 5]   '   5_      � 5b       5c   "   k� 5d   &    �    �      �   5g       5i   "   u� 5j   &    66   �� *
faces[3]+# [ 5l   '   5n      � 5q       5r   "   k� 5s   &    �    �      �   5v       5x   "   u� 5y   &    66   �� *
faces[5]+# [ 5z   '   5|      � 5       5�   "   k� 5�   &    �    �      �   5�       5�   "   u� 5�   &    66   �� *
faces[4]+# [ 5�   '   5�      5�      5�      5�      5�      5�      