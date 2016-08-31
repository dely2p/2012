module clock(clk, rst, seg_com, seg_data, beep);

input clk, rst;
output [7:0] seg_data, seg_com;
output beep;

reg [7:0] seg_data, seg_com;
reg [3:0] locate_c;

wire [7:0] seg_data_s_1, seg_data_s_10, seg_data_m_1, seg_data_m_10,
			  seg_data_h_1, seg_data_h_10, seg_data_ap, seg_data_d_1, seg_data_d_2;
wire clk_m, clk_h, clk_ap, clk_b;


second_digit U_dis_sec (clk, rst, clk_m, seg_data_s_1, seg_data_s_10);

dot_digit U_dis_dot (clk, rst, seg_data_d_1, seg_data_d_2);

minute_digit U_dis_min (clk_m, rst, clk_h, clk_b,seg_data_m_1, seg_data_m_10);

hour_digit U_dis_hour (clk_h, rst, clk_ap, seg_data_h_1, seg_data_h_10);

ampm_digit U_ampm (clk_ap, rst, seg_data_ap);

beep_digit U_beep (clk, clk_b, beep);
always @(posedge clk) begin

	if(locate_c == 8)
	locate_c <= 0;
	else
	locate_c <= locate_c + 1;
	
	case(locate_c)
			 4'd0 : begin
				seg_data <= seg_data_s_1;
				seg_com <= 8'b0111_1111;
				end
			 4'd1 : begin
				seg_data <= seg_data_s_10;
			   seg_com <= 8'b1011_1111;
				end
			 4'd2 : begin
				seg_data <= seg_data_d_1;
				seg_com <= 8'b1101_1111;
				end	
			 4'd3 : begin
				seg_data <= seg_data_m_1;
				seg_com <= 8'b1101_1111;
				end
			 4'd4 : begin
				seg_data <= seg_data_m_10;
				seg_com <= 8'b1110_1111;
				end
			 4'd5 : begin
				seg_data <= seg_data_h_1;
				seg_com <= 8'b1111_0111;
				end
			 4'd6 : begin
				seg_data <= seg_data_d_2;
				seg_com <= 8'b1111_0111;
				end
			 4'd7 : begin
				seg_data <= seg_data_h_10;
				seg_com <= 8'b1111_1011;
				end
			 4'd8 : begin
				seg_data <= seg_data_ap;
				seg_com <= 8'b1111_1101;
				end
	endcase
end

endmodule
