I:
0 Repo - > cahnge Sector and User interfaces to generic Edit repo Select by ID delete by ID
0 WCF - Add WCF between Repo and Site

0 PresentationSite - parsing excel file to objects method decompose to several
	///Sector type from filename
	///Fist file input determines merchant sector
	parse file mask;
	
	if parsed then
	{
		get sector id from filename; --from config
		get property fields; --from config
		if(no merchant column then error); --from parser

		foreach row
		{
			check merchant doubles for this sector; --from UOF
			remove doubles; --from UOF
			insert; --from UOF
		}
		
	}

0 PresentationSite - Export to excel
0 SB to console

SB , Repo and UOF to different projects  Repo to DLL <- done

II:
0 Polinom Parse - parse *^/ to expressions and add priorities for exprs
0 Command line app dll
0 Multithread message ping pong

III:
0 Type converter for migration -> (
Add SQL to CLR type conversion ; 
SQLEntity to DWH entity converter; 
Converter logic to string name or type name;
)

Tables migration - test with repo <- done
