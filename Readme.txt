Manejo de Rutas de Vuelos New Shore
Aplicacion Web para manejo de rutas de vuelos compañia New SHore
Basada en NET CORE 5.0
Consume API Rest
https://recruiting-api.newshore.es/api/flights
Esta es consumida a su vez por un API Rest NEWSHORE_AIR API
API NEWSHORE_AIR
Basada en API Rest desarrollada con NET CORE 5.0 usando Json
Se usa inyeccion de dependencias y singleton para el metodo Get_NewShoreAir
Se parametriza en appsettings las url https://recruiting-api.newshore.es/api/flights para futuras actualizaciones
4 Metodos
GET
​/NewShore​/Get_NewShoreAir
Consume API https://recruiting-api.newshore.es/api/flights)
GET
​/NewShore​/Origin
Extrae los origenes de la data
GET
​/NewShore​/Destination
Extrae los destinos de la data
GET
​/NewShore​/Rutas
Calcula las rutas
NEW_SHORE_UI
Front basada en CORE MVC con dos listas desplegables construidas en base a los origenes y destino provenientes de la data suministrada por el API consumida, area de visualizacion de la ruta calculada y boton CALCULAR RUTA
Se usa inyeccion de dependencias para el uso de los metodos de la API NEWSHORE_AIR
Se realiza control de algunas excepciones
Se usan listas para minimizar errores de digitacion usando cuadros de texto propuestos

Diseño basado en la teoria de grafos aplicada a calculo de rutas
Calcula rutas de hasta tres escalas
Exceptua escalas internacionales cuando el viaje es local, en este caso a Colombia


Rutas de Prueba
MZL MED      CTG MAD             MZL BCN             PEI MAD   
               CTG BOG MAD          MZL MDE BCN        PEI BOG MAD

MED PEI               MDE BOG
 MDE MZL PEI           MDE CTG BOG
 MDE CTG MZL PEI       MDE MZL PEI BOG
                       MDE MZL CTG BOG

PEI BCN                    MZL MEX               MZL MAD
 PEI MZL MDE BCN             MZL PEI BOG MEX       MZL CTG BOG MAD
 PEI MZL CTG MDE BCN         MZL CTG BOG MEX       MZL PEI BOG MAD

BCN PEI
 BCN MDE MZL PEI

MED MAD
MED CTG BOG MAD
MDE MZL PEI BOG MAD
MDE MZL CTG BOG MAD



