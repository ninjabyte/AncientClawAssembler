;; Sample

;; Add two numbers and display them

LET16 A
.DB16 (77)
LET16 A
.DB16 (1223)
ADD16 A
DMPSSTR
.DBS "77 + 1223 ="
DMPN16 A