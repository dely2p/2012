module ap_seg_mod(bcd_in, seg_data);

input bcd_in;
output [7:0] seg_data;

reg [7:0] seg_data;

always @ (bcd_in) begin
		case (bcd_in)
				4'b0000 : seg_data = 8'b0111_0111;
				4'b0001 : seg_data = 8'b0111_0011;
				
				default : seg_data = 8'b0000_0000;
	  endcase
end

endmodule
