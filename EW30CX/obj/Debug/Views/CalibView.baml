   M S B A M L   `   `   `  ����A  =EW30CX, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:EW30CX.ViewsEW30CX.Views  5          EW30CX.Views.CalibView   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      ,localclr-namespace:EW30CX.Views     5      
�������.��5
      � $��5,5,5,0q�6     � RowDefinitions  5   
   �� 5      $��50 �6   �� 5      $��* �6   .+�5   
   �� ��6   �� *StartCommand+��62   ��  �� Path$ CM.buttonStartContent� �� UpdateSourceTrigger$ PropertyChanged=���5      �� 5       �� 
TargetType" ��6   .�5      ��  �� Property � �� �� Value$ 17(�61   �� 5       � ��$ SemiBold"�63   �� 5       � ��$ 40��6/   �� 5       � ��$	 120��6.   �� 5       � ��$
 Left=�6<    �� Triggers 5      �� 5       �� Binding 6&   ��  �� RelativeSource �� 	 �� Mode$
	 Self=�$ Content�
 �� Value	
 START6r   .a�5      ��  � {� . 6:   5      �� 5       6&   ��  �� $
	 Self=�$ Content�
 STOP6r   .a�5      ��  � {� .  6:   5      5!      5#      �� 5%   
   $��1��6    �� TabStripPlacement$ Bottom=�6"   .@�5&      �� ��5'      �� 5(      $��14(�6   $��Log Item��6*    � 	Resources 5+         
label_name  color_border  label_content  color_ready  
color_pass  
color_fail  
color_wait  
label_time��%�  &	        7  &	 )    7  7  7  7  7  &	 F    7  �� 5-      " ��6   .�5.      ��  � ��$ 14(�65   �� 5/       � ��8   68   �� 50       � ��$ 25��63   �� 51       � ��$ 1,0,1,1q�6<   �� 52       � ��$	 220��62   �� 53       � ��$
 Left=�6@   �� 54       � ��$ 5,2,0,0q�64   55      �� 57      " ��6   .�58      ��  � ��$ 17(�65   �� 59       � ��8   68   �� 5:       � ��$ 0,0,1,1q�6<   �� 5;       � ��$	 100��62   �� 5<       � ��$ 25��63   �� 5=       � ��$ SemiBold"�67   �� 5>       � ��$
 Left=�6@   �� 5?       � ��$ 	220,0,0,0q�63   �� 5@       � ��$ 5,0,0,0q�64   �� 5A       � ��8  67    5B      �� 5C       6*   ��  �� $
	 Self=�$ Content�

 Passed6v   .a�5D   "   ��  � ��8  6?   5E      �� 5F       6*   ��  �� $
	 Self=�$ Content�

 Failed6v   .a�5G   "   ��  � ��8  6?   5H      �� 5I       6*   ��  �� $
	 Self=�$ Content�
 
Waiting...6v   .a�5J   "   ��  � ��8  6?   5K      5M      �� 5O      " ��6   .�5P      ��  � ��$ 14(�65   �� 5Q       � ��8   68   �� 5R       � ��$ 0,0,1,1q�6<   �� 5S       � ��$ 70��62   �� 5T       � ��$ 25��63   �� 5U       � ��$
 Left=�6@   �� 5V       � ��$ 	320,0,0,0q�63   �� 5W       � ��$ 5,2,0,0q�64   5X      5Z      .��5\      �  � ColumnDefinitions 5]      �� 5^      $	��450 �6+   �� 5_      $��* �6+   .+�5b      �� $��0,5,0,0q�6!   .+�5c      �� $	��390��6!   $
��Left=�6-   #��[ 6H   $��0,0,0,1q�6t   � 5d      .+�5e      �� $��Mở cổng com dut��6$   #��[  6>   �� 5f      #��[ 6n   ��6$   �� *CM.openComResult+$ PropertyChanged=�6n   �� 5g      #��[ 6s   ��6$   �� *CM.openComElapsedTime+$ PropertyChanged=�6s   5h      � 5i      .+�5j      �� $��Chờ dut khởi động��6$   #��[  6@   �� 5k      #��[ 6s   ��6$   �� *CM.bootCompleteResult+$ PropertyChanged=�6s   �� 5l      #��[ 6$   ��6H   �� *CM.bootCompleteElapsedTime+$ PropertyChanged=�5m      � 5n      .+�5o      �� $��Đăng nhập vào dut��6$   #��[  6@   �� 5p      #��[ 6l   ��6$   �� *CM.loginResult+$ PropertyChanged=�6l   �� 5q      #��[ 6$   ��6H   �� *CM.loginElapsedTime+$ PropertyChanged=�5r      � 5s      .+�5t      �� $��Đọc mac wan của dut��6$   #��[  6B   �� 5u      #��[ 6p   ��6$   �� *CM.getMacWanResult+$ PropertyChanged=�6p   �� 5v      #��[ 6$   ��6H   �� *CM.getMacWanElapsedTime+$ PropertyChanged=�5w      � 5x      .+�5y      �� $"��Mac wan không phải golden��6$   #��[  6H   �� 5z      #��[ 6r   ��6$   �� *CM.checkGoldenResult+$ PropertyChanged=�6r   �� 5{      #��[ 6$   ��6H   �� *CM.checkGoldenElapsedTime+$ PropertyChanged=�5|      � 5}      $��0,0,60,0q�6   ��5~      �� 5   "   " �6(   .�5�   &   ��  � ��
 �����6C    5�   &   �� 5�   *    66   �� $ SM.isChangeIp�$ PropertyChanged=�	
 False6�   .a�5�   .   ��  � ��  color_disable# [ 6K   5�   +   5�   #   .+�5�      �� $��Đổi ip ethernet dut��6$   #��[  6B   �� 5�      #��[ 6o   ��6$   �� *CM.changeIPResult+$ PropertyChanged=�6o   �� 5�      #��[ 6$   ��6H   �� *CM.changeIPElapsedTime+$ PropertyChanged=�5�      � 5�      $��0,0,60,0q�6   ��5�      �� 5�   "   " �6(   .�5�   &   ��  � ��
 �����6C    5�   &   �� 5�   *    66   �� $ SM.isChangeIp�$ PropertyChanged=�	
 False6�   .a�5�   .   ��  � ��# [ 6K   5�   +   5�   #   .+�5�      �� $0��*Ping tới địa chỉ ip mới của dut��6$   #��[  6N   �� 5�      #��[ 6n   ��6$   �� *CM.pingDUTResult+$ PropertyChanged=�6n   �� 5�      #��[ 6$   ��6H   �� *CM.pingDUTElapsedTime+$ PropertyChanged=�5�      � 5�      .+�5�      �� $ ��Chuyển dut sang FTM mode��6$   #��[  6G   �� 5�      #��[ 6q   ��6$   �� *CM.switchModeResult+$ PropertyChanged=�6q   �� 5�      #��[ 6$   ��6H   �� *CM.switchModeElapsedTime+$ PropertyChanged=�5�      � 5�      $��0,0,60,0q�6   ��5�      �� 5�   "   " �6(   .�5�   &   ��  � ��
 �����6C    5�   &   �� 5�   *    66   �� $ SM.isRunTesttree�$ PropertyChanged=�	
 False6�   .a�5�   .   ��  � ��# [ 6K   5�   +   5�   #   .+�5�      �� $ ��Calib công suất 2G & 5G��6$   #��[  6J   �� 5�      #��[ 6p   ��6$   �� *CM.calibWlanResult+$ PropertyChanged=�6p   �� 5�      #��[ 6$   ��6H   �� *CM.calibWlanElapsedTime+$ PropertyChanged=�5�      � 5�      $��0,10,0,0q�6$   .+�5�      �� $��Mac wan sản phẩm: ��6$   $��14(�6A   �� 5�      $��SemiBold"�6g   $��14(�6}   ��6$   �� *	CM.macWan+$ PropertyChanged=�6}   5�      � 5�      $��0,-5,0,0q�6$   .+�5�      �� $��Tổng thời gian: ��6$   $��14(�6?   �� 5�      $��SemiBold"�6j   $��14(�6�   ��6$   �� *CM.totalTime+$ PropertyChanged=�6�   5�      �� 5�      $��0,-5,0,0q�6    $��Kết quả: ��62   $��14(�6F   �� 5�      ��6    �� *CM.totalResult+$ PropertyChanged=���5�      �� 5�   "   " ��6(   .�5�   &   ��  � ��$ 	0,-15,0,0q�6?   �� 5�   &    � ��$ 50(�6A   �� 5�   &    � ��$ SemiBold"�6C   �� 5�   &    � ��# [ 6C    5�   &   �� 5�   *    66   ��  �� $
	 Self=�$ Content�

 Passed6�   .a�5�   .   ��  � ��# [ 6K   5�   +   �� 5�   *    66   ��  �� $
	 Self=�$ Content�

 Failed6�   .a�5�   .   ��  � ��# [ 6K   5�   +   �� 5�   *    66   ��  �� $
	 Self=�$ Content�
 
Waiting...6�   .a�5�   .   ��  � ��# [ 6K   5�   +   5�   #   5�      5�      � 5�      $��1��6    	 color_background#��[	 6+   .+�5�      �� .��5�      �� $��10,0,0,0q�6)    5�   "    
 label_title  label_setting  textblock_setting P� Property P� Value��%+  &	
       &	 H     &	 �     �� 5�   &   " ��6,   .�5�   *   ��  � ��$ 14(�6E   5�   '   �� 5�   &   " ��6,   .�5�   *   ��  � ��$ 14(�6E   �� 5�   *    � ��$ SemiBold"�6G   5�   '   �� 5�   &   " ��6,   .�5�   *   ��  � ��$ 14(�6E   �� 5�   *    � ��$
 Wrap=�6I   �� 5�   *    � ��$ SemiBold"�6G   �� 5�   *    � ��
 �   �6G   �� 5�   *    �@�Cursor$
 Hand��6C    5�   *   P� 5�   .    �@J�IsMouseOver . . �5�   2   ��  � ��
 �  ��6O   5�   /   5�   '   5�   #   .+�5�   "   � .+�5�   &   �� #��[
 6,   $��Địa chỉ ip calib:��6Q   �� 5�   &   #��[ 6,   ��6S   �� *
SM.ipDUT+$ PropertyChanged=�5�   #   � 5�   "   .+�5�   &   �� #��[
 6,   $��Cổng com sản phẩm:��6Q   �� 5�   &   #��[ 6,   ��6S   �� *
SM.comPort+$ PropertyChanged=�5�   #   � 5�   "   .+�5�   &   �� #��[
 6,   $��Còn lại:��6Q   �� 5�   &   #��[ 6,   ��6S   �� $ SM.qty_remain�$ PropertyChanged=�5�   #   � 5�   "   .+�5�   &   �� #��[
 6,   $-��'Độ lệch công suất golden (dBm):��6Q   �� 5�   &   #��[ 6,   ��6S   �� *SM.powerDiffGolden+$ PropertyChanged=�5�   #   � 5�   "   .+�5�   &   �� #��[
 6,   $*��$Số lượng golden verify suy hao:��6Q   �� 5�   &   #��[ 6,   ��6S   �� *SM.qtyGoldenVerify+$ PropertyChanged=�5�   #   � 5   "   .+�5  &   �� #��[
 6,   $ ��Golden đo suy hao trạm:��6Q   �� 5  &   #��[ 6,   ��6S   �� $ SM.golden_attenuation�$ PropertyChanged=�5  #   �� 5  "   #��[
 6(   $#��Golden verify suy hao trạm:��6M   �� 5  "   #��[ 6,   ��6W   �� $ SM.golden_verification�$ PropertyChanged=��� 5  "   #��[
 6(   $"��Testtree calib công suất:��6M   �� 5  "   #��[ 6,   ��6W   �� *SM.calibTesttreeFile+$ PropertyChanged=� J� InputBindings 5	  &   d� 5
  *    d� MouseAction$ 	LeftClicke�6d    �� Command 67   �� *OpenCalibTestTreeCommand+6d   5  #   �� 5  "   #��[
 6(   $��Testtree đo suy hao:��6M   �� 5  "   #��[ 6,   ��6W   �� *SM.attTesttreeFile+$ PropertyChanged=� 5  &   d� 5  *   $ 	LeftClicke�6b    67   �� *OpenAttTestTreeCommand+6b   5  #   �� 5  "   #��[
 6(   $��	Pathloss:��6M   �� 5  "   #��[ 6,   ��6W   �� *SM.pathLossFile+$ PropertyChanged=� 5  &   d� 5  *   $ 	LeftClicke�6_    67   �� *OpenPathlossCommand+6_   5  #   �� 5  "   $�� ��6(   $��30��63   5     5     5     5     5     �� 5!     ��5"     �� 5#     $��14(�6   $��
Log System��6*   .��5&     �� -   5         ��Name$ 
scr_system��5&     .��5'     ��  �� 
IsReadOnly . $��0,0,1,2q�64   $��5,5,0,0q�6E   $��Stretch=�6W   $��Stretch=�6s   . �5(     /� .T�5)     J� .*�6(   .*�5*  "   �� �6&   �� $ CM.logSystem�$ PropertyChanged=�5+     5,     5-     5.     50     �� 52     ��53     �� 54     $��14(�6   $��Log QSPR��6*   .��57     �� -   5        $ scr_qspr��57     .��58     ��  . $��0,0,1,2q�64   $��5,5,0,0q�6E   $��Stretch=�6W   $��Stretch=�6s   . �59     /� .T�5:     J� .*�6(   .*�5;  "   �� �6&   �� $ CM.logQSPRMini�$ PropertyChanged=�5<     5=     5>     5?     5A     �� 5C     ��5D     �� 5E     $��14(�6   $��Log Dut��6*   .��5H     �� -   5        $ scr_dut��5H     .��5I     ��  . $��0,0,1,2q�64   $��5,5,0,0q�6E   $��Stretch=�6W   $��Stretch=�6s   . �5J     /� .T�5K     J� .*�6(   .*�5L  "   �� �6&   �� $ 	CM.logDUT�$ PropertyChanged=�5M     5N     5O     5P     5Q     5S     5T     5V     