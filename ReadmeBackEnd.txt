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
Consume API https://recruiting-api.newshore.es/api/flights
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
Front basada en NET CORE MVC con dos listas desplegables construidas en base a los origenes y destino provenientes de la data suministrada por el API consumida, area de visualizacion de la ruta calculada y boton CALCULAR RUTA
Se usa inyeccion de dependencias para el uso de los metodos de la API NEWSHORE_AIR
Se realiza control de algunas excepciones
Se usan listas para minimizar errores de digitacion usando cuadros de texto propuestos
Uso de Clases anidadas y/o enlazadas
Diseño basado en la teoria de grafos aplicada a calculo de rutas
Calcula rutas de hasta dos escalas
Calcula una ruta alternativa en el caso de que exista con sus escalas si asi fuese
Exceptua escalas internacionales cuando el viaje es local, en este caso a Colombia
Uso de Clases de BootStrap
Uso de Ajax 
Uso de JQuery
Uso de SwalAlert para los mensajes
Rutas de Prueba

BACKEND
Uso de carga DILIGENTE para traer la data de la DB cuando ya la ruta se ha calculado previamente y se encuentra almacenada en la base de datos
Adicion de rutas calculadas no existente en la base de datos condicionadas a la no existencia de las mismas
Uso de REPOSITORY

MZL MED     MED CTG    PEI CTG          MZL BOG             CTG MAD             MZL BCN             PEI MAD            PEI MEX   
                          PEI MZL CTG     MZL CTG BOG         CTG BOG MAD         MZL MDE BCN         PEI BOG MAD        PEI BOG MEX
                          PEI BOG CTG     MZL PEI BOG 
                                          MZL MDE CTG BOG

MED PEI               MDE BOG                  BOG BCN                    MED MAD                   MAD MED
 MDE MZL PEI           MDE CTG BOG               BOG CTG MDE BCN             MDE CTG BOG MAD          MAD BOG CTG MDE
 MDE CTG MZL PEI       MDE MZL PEI BOG           BOG PEI MZL MDE BCN         MDE MZL PEI BOG MAD
                       MDE MZL CTG BOG           BOG CTG MZL MDE BCN         MDE MZL CTG BOG MAD

PEI BCN                 BCN PEI              PEI CAN              MZL MEX               MEX MZL               MZL MAD
  PEI MZL MDE BCN         BCN MDE MZL PEI      PEI MZL CTG CAN      MZL PEI BOG MEX       MEX BOG PEI MZL       MZL CTG BOG MAD
  PEI MZL CTG MDE BCN                          PEI BOG CTG CAN      MZL CTG BOG MEX       MEX BOG CTG MZL       MZL PEI BOG MAD

[{"departureStation":"MZL","arrivalStation":"MDE","flightCarrier":"CO","flightNumber":"8001","price":200},{"departureStation":"MZL","arrivalStation":"CTG","flightCarrier":"CO","flightNumber":"8002","price":200},{"departureStation":"PEI","arrivalStation":"BOG","flightCarrier":"CO","flightNumber":"8003","price":200},{"departureStation":"MDE","arrivalStation":"BCN","flightCarrier":"CO","flightNumber":"8004","price":500},{"departureStation":"CTG","arrivalStation":"CAN","flightCarrier":"CO","flightNumber":"8005","price":300},{"departureStation":"BOG","arrivalStation":"MAD","flightCarrier":"CO","flightNumber":"8006","price":500},{"departureStation":"BOG","arrivalStation":"MEX","flightCarrier":"CO","flightNumber":"8007","price":300},{"departureStation":"MZL","arrivalStation":"PEI","flightCarrier":"CO","flightNumber":"8008","price":200},{"departureStation":"MDE","arrivalStation":"CTG","flightCarrier":"CO","flightNumber":"8009","price":200},{"departureStation":"BOG","arrivalStation":"CTG","flightCarrier":"CO","flightNumber":"8010","price":200},{"departureStation":"MDE","arrivalStation":"MZL","flightCarrier":"CO","flightNumber":"9001","price":200},{"departureStation":"CTG","arrivalStation":"MZL","flightCarrier":"CO","flightNumber":"9002","price":200},{"departureStation":"BOG","arrivalStation":"PEI","flightCarrier":"CO","flightNumber":"9003","price":200},{"departureStation":"BCN","arrivalStation":"MDE","flightCarrier":"ES","flightNumber":"9004","price":500},{"departureStation":"CAN","arrivalStation":"CTG","flightCarrier":"MX","flightNumber":"9005","price":300},{"departureStation":"MAD","arrivalStation":"BOG","flightCarrier":"ES","flightNumber":"9006","price":500},{"departureStation":"MEX","arrivalStation":"BOG","flightCarrier":"MX","flightNumber":"9007","price":300},{"departureStation":"PEI","arrivalStation":"MZL","flightCarrier":"CO","flightNumber":"9008","price":200},{"departureStation":"CTG","arrivalStation":"MDE","flightCarrier":"CO","flightNumber":"9009","price":200},{"departureStation":"CTG","arrivalStation":"BOG","flightCarrier":"CO","flightNumber":"9010","price":200}]


new Route { departureStation = "MZL", arrivalStation = "CTG", flightCarrier = "CO",flightNumber = "8002", price = 200},
new Route { departureStation = "PEI", arrivalStation = "BOG", flightCarrier = "CO",flightNumber = "8003", price = 200},
new Route { departureStation = "MDE", arrivalStation = "BCN", flightCarrier = "CO",flightNumber = "8004", price = 500},
new Route { departureStation = "CTG", arrivalStation = "CAN", flightCarrier = "CO",flightNumber = "8005", price = 300},
new Route { departureStation = "BOG", arrivalStation = "MAD", flightCarrier = "CO",flightNumber = "8006", price = 500},
new Route { departureStation = "BOG", arrivalStation = "MEX", flightCarrier = "CO",flightNumber = "8007", price = 300},
new Route { departureStation = "MZL", arrivalStation = "PEI", flightCarrier = "CO",flightNumber = "8008", price = 200},
new Route { departureStation = "MDE", arrivalStation = "CTG", flightCarrier = "CO",flightNumber = "8009", price = 200},
new Route { departureStation = "BOG", arrivalStation = "CTG", flightCarrier = "CO",flightNumber = "8010", price = 200},
new Route { departureStation = "MDE", arrivalStation = "MZL", flightCarrier = "CO",flightNumber = "9001", price = 200},
new Route { departureStation = "CTG", arrivalStation = "MZL", flightCarrier = "CO",flightNumber = "9002", price = 200},
new Route { departureStation = "BOG", arrivalStation = "PEI", flightCarrier = "CO",flightNumber = "9003", price = 200},
new Route { departureStation = "BCN", arrivalStation = "MDE", flightCarrier = "ES",flightNumber = "9004", price = 500},
new Route { departureStation = "CAN", arrivalStation = "CTG", flightCarrier = "MX",flightNumber = "9005", price = 300},
new Route { departureStation = "MAD", arrivalStation = "BOG", flightCarrier = "ES",flightNumber = "9006", price = 500},
new Route { departureStation = "MEX", arrivalStation = "BOG", flightCarrier = "MX",flightNumber = "9007", price = 300},
new Route { departureStation = "PEI", arrivalStation = "MZL", flightCarrier = "CO",flightNumber = "9008", price = 200},
new Route { departureStation = "CTG", arrivalStation = "MDE", flightCarrier = "CO",flightNumber = "9009", price = 200},
new Route { departureStation = "CTG", arrivalStation = "BOG", flightCarrier = "CO",flightNumber = "9010", price = 200},



