@ECHO OFF
for %%f in (*.pdf) do (

 echo %%~nf
 java -Durl=http://localhost:8983/solr/update/extract?literal.id=%%~nf -Dtype=application/pdf -jar post.jar %%~nf.pdf	    
)
