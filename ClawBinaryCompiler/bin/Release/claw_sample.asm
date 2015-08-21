;; Claw Demo program
;; (c) 2015 Benedikt Muessig
;; Licenced under the terms of the MIT Licence

;; This program calculates a power purely in software

LET16 A
.DB16U (5)	; m
LET16 B
.DB16 (3)	; n

LET16 B
.DB16 (-1)
ADD16 B

LET32 B
.DB32 ()
br B

