
using System;
using System.Collections;
using System.Collections.Generic;
using tp1;

namespace MyApp
{

	public class Estrategia
	{
		private int CalcularDistancia(string str1, string str2)
		{
			// using the method
			String[] strlist1 = str1.ToLower().Split(' ');
			String[] strlist2 = str2.ToLower().Split(' ');
			int distance = 1000;
			foreach (String s1 in strlist1)
			{
				foreach (String s2 in strlist2)
				{
					distance = Math.Min(distance, Utils.calculateLevenshteinDistance(s1, s2));
				}
			}

			return distance;
		}

        public String Consulta1(ArbolGeneral<DatoDistancia> arbol) //PUNTO 3
        {
            string oracion1 = "Esta es la oracion que se formo:"; //creo una oracion inicial
            string oracion2 = ""; //creo una oracion donde voy a agrupar los texto de cada nodo
            if (arbol.esHoja())
            {
                oracion2 = oracion2 + " " + arbol.getDatoRaiz().texto;
            }
            else
            {
                Cola<ArbolGeneral<DatoDistancia>> cola = new Cola<ArbolGeneral<DatoDistancia>>(); //creo una cola
                ArbolGeneral<DatoDistancia> arbolaux; // creo un arbol auxiliar que servira de puntero
                cola.encolar(arbol); //encolo el arbol
                while (!cola.esVacia()) //mientras la cola no se vacie
                {
                    arbolaux = cola.desencolar(); // desencolo el arbol en el arbol auxiliar
                    foreach (var hijo in arbolaux.getHijos()) //tomo la lista de hijos
                    {
                        if (hijo.esHoja()) //si ese hijo es hoja
                        {
                            oracion2 = oracion2 + " " + hijo.getDatoRaiz().texto; //concatena todos los texto de las hojas
                        }
                        cola.encolar(hijo);//encola el hijo para seguir buscando mas hojas
                    }
                }
            }

            string oracion = oracion1 + oracion2;//concatena la oracion inial con el texto de las hojas
            return oracion; //retorna la oracion
        }




        private void caminos(ArbolGeneral<DatoDistancia> arbol, ArrayList listadelistas, ArrayList listadestrings)
        {
            if (arbol.esHoja())
            {
                listadestrings.Add(arbol.getDatoRaiz().texto);
                ArrayList aux = new ArrayList();
                foreach (string ele in listadestrings)
                {
                    aux.Add(ele);
                }
                listadelistas.Add(aux);

            }
            else
            {
                listadestrings.Add(arbol.getDatoRaiz().texto);
                foreach (ArbolGeneral<DatoDistancia> hijo in arbol.getHijos())
                {
                    caminos(hijo, listadelistas, listadestrings);
                    listadestrings.RemoveAt(listadestrings.Count - 1);
                }
            }
        }
        public String Consulta2(ArbolGeneral<DatoDistancia> arbol) //PUNTO 4
        {
            int contador = 0;
            string oracion1 = "camino hacia las hojas"; //creo oracion inial
            string oracion = ""; //creo una oracion donde se van a guardar el texto de cada nodo hacia las hojas
            ArrayList listadelistas = new ArrayList();
            ArrayList listadstrings = new ArrayList();
            caminos(arbol, listadelistas, listadstrings);
            foreach (ArrayList lista in listadelistas)
            {
                oracion = oracion + " LISTA: " + contador + " ";
                foreach (string nodo in lista)
                {

                    oracion = oracion + nodo + " , ";
                }
                oracion = oracion + "///";
                contador++;
            }
            oracion1 += oracion;
            return oracion1; //retorna oracion
        }



        public String Consulta3(ArbolGeneral<DatoDistancia> arbol) // PUNTO 5
        {
            DatoDistancia datoaux = new DatoDistancia(999, "AAA", "AAA"); //creo un DatoDistancia que va a funcionar como flag
            ArbolGeneral<DatoDistancia> contador = new ArbolGeneral<DatoDistancia>(datoaux); //arbolgeneral del DatoDistancia para que la cola lo admita
            int nivel = -1; //contador de niveles, inicializado en -1 para que al menos haga la iteracion en el nivel 0
            string oracion1 = "Esta es la oracion que se formo: "; //creo una oracion inial
            string oracion2 = ""; //creo una oracion donde se van a guardar el texto de cada nodo hacia las hojas
            if (arbol.esHoja()) //pregunto si es hoja
            {
                nivel++; //subo el nivel para que quede en 0
                oracion2 = oracion2 + "nivel: " + nivel + " ,Distancia: " + arbol.getDatoRaiz().distancia + " ,Texto: " + arbol.getDatoRaiz().texto + " ,Descripcion: " + arbol.getDatoRaiz().descripcion; //concateno la oracion con los datos del DatoDistancia
            }
            else//si no es
            {
                nivel++; //subo el nivel
                Cola<ArbolGeneral<DatoDistancia>> cola = new Cola<ArbolGeneral<DatoDistancia>>();  //creo una cola
                ArbolGeneral<DatoDistancia> arbolaux;  // creo un arbol auxiliar que servira de puntero
                cola.encolar(arbol); //encolo el arbol pasado como parametro
                cola.encolar(contador); //encolo el flag
                while (!cola.esVacia()) //mientras la cola no este vacia
                {
                    arbolaux = cola.desencolar(); //desencolo el primer elemento de la cola
                    if (arbolaux != contador) //comparo si es distinto al flag
                    {
                        oracion2 = oracion2 + " Nivel: " + nivel + " ,Distancia: " + arbolaux.getDatoRaiz().distancia + " ,Texto: " + arbolaux.getDatoRaiz().texto + " ,Descripcion: " + arbolaux.getDatoRaiz().descripcion; //concateno la oracion con los datos del DatoDistancia
                        foreach (var hijo in arbolaux.getHijos())
                        { //pido la lista de hijos
                            cola.encolar(hijo); //los encolo
                        }
                    }
                    else
                    { //si es igual al flag 
                        nivel++; //subo el nivel
                        if (!cola.esVacia())
                        { //pregunto si la cola esta vacia
                            cola.encolar(contador); //encolo un flag
                        }
                    }
                }
            }
            string oracion = oracion1 + oracion2; //concateno las oraciones
            return oracion; //retorno las oraciones
        }

        public void AgregarDato(ArbolGeneral<DatoDistancia> arbol, DatoDistancia dato) //PUNTO 1
        {
            int distancia = CalcularDistancia(arbol.getDatoRaiz().texto, dato.texto);
            Cola<ArbolGeneral<DatoDistancia>> cola = new Cola<ArbolGeneral<DatoDistancia>>(); //creacion de la cola para recorrer el arbol general por niveles
            ArbolGeneral<DatoDistancia> arbolaux; //arbol auxiliar que nos va a servir de puntero
            cola.encolar(arbol); //encolo el arbol que pase como parametro
            while (!cola.esVacia())
            {
                arbolaux = cola.desencolar(); //desencolo el arbol auxiliar
                foreach (var hijo in arbolaux.getHijos())
                { //tomo la lista de hijos
                    cola.encolar(hijo); //y los encolo
                }
                if (distancia == arbolaux.getDatoRaiz().distancia)
                {
                    AgregarDato(arbolaux, dato);
                }
            }
            if (distancia != arbol.getDatoRaiz().distancia)
            {
                ArbolGeneral<DatoDistancia> nuevonodo = new ArbolGeneral<DatoDistancia>(dato); //se crea un arbol genera con el DatoDistancia
                arbol.agregarHijo(nuevonodo); //se agrega ese arbol al arbol que se paso como parametro
            }

        }

        public void Buscar(ArbolGeneral<DatoDistancia> arbol, string elementoABuscar, int umbral, List<DatoDistancia> collected) //PUNTO 2
        {
            int nivel = -1; //contador de niveles, inicializado en -1 para que al menos haga la iteracion en el nivel 0
            string oracion = ""; //string que va imprimir por pantalla
            DatoDistancia datoaux = new DatoDistancia(999, "AAA", "AAA"); //DatoDistancia que va a actuar como un flag
            ArbolGeneral<DatoDistancia> arbolcontador = new ArbolGeneral<DatoDistancia>(datoaux); //arbolgeneral del DatoDistancia para que la cola lo admita
            Cola<ArbolGeneral<DatoDistancia>> cola = new Cola<ArbolGeneral<DatoDistancia>>(); //creacion de la cola para recorrer el arbol general por niveles
            ArbolGeneral<DatoDistancia> arbolaux; //arbol auxiliar que nos va a servir de puntero
            cola.encolar(arbol); //encolo el arbol que pase como parametro
            while (!cola.esVacia())
            { //hasta que la cola no se termina el while no se termina
                arbolaux = cola.desencolar(); //desencolo el arbol auxiliar
                if (arbolaux.getDatoRaiz().texto == elementoABuscar)
                { //pregunto si el texto del DatoDistancia del arbol es igual al elemento a buscar
                    collected.Add(arbolaux.getDatoRaiz()); //si es igual al elemento a buscar lo agrego a la lista
                    Cola<ArbolGeneral<DatoDistancia>> cola2 = new Cola<ArbolGeneral<DatoDistancia>>(); //creo una lista para recorrer los hijos del elemento
                    ArbolGeneral<DatoDistancia> arbolaux2; //arbol auxiliar 2 que nos va a servir de puntero
                    cola2.encolar(arbolaux); //encolo el arbol que tiene el elemento
                    cola2.encolar(arbolcontador); //encolo el arbol que nos va a servir como flag
                    while (!cola2.esVacia())
                    { //pregunto si la cola esta vacia
                        arbolaux2 = cola2.desencolar(); //desencolo el arbol auxiliar2 ------------------------------------------------------------------------------------------------------------
                        if (nivel <= umbral)
                        { //si el nivel es menor o igual umbral sigo
                            if (arbolaux2 != arbolcontador)
                            { //si el arbol auxiliar 2 es diferente al flag
                                foreach (var hijo2 in arbolaux2.getHijos())
                                { //toma la lista de hijos
                                    cola2.encolar(hijo2); //los encola
                                    collected.Add(hijo2.getDatoRaiz()); //agrego el DatoDistancia a la list
                                }
                            }
                            else
                            { //si es igual al flag
                                nivel++; //sube 1 el contador de nivel
                                if (!cola2.esVacia())
                                { //mientas la cola2 no este vacia 
                                    cola2.encolar(arbolcontador); //encolo otro flag
                                }
                            }
                        }
                    }
                }
                else
                { //si no es igual 
                    foreach (var hijo in arbolaux.getHijos())
                    { //tomo la lista de hijos
                        cola.encolar(hijo); //y los encolo
                    }
                }
            }
            if (collected.Count == 0)
            { //no se encontro el elemento por lo tanto, no hay ningun elemento en la list
                Console.WriteLine("El elemento no se encontro"); //imprime por pantalla el aviso
            }
            foreach (var ele in collected)
            { //se recorre la lista
                oracion = oracion + ele.texto + " , "; //se forma la oracion con los textos de cada DatoDistancia
            }
            Console.WriteLine(oracion); //imprime la oracion formada
        }

    }


    public void Busqueda_de_caminos(ArbolGeneral<DatoDistancia> arbol, ArrayList lista1, ArrayList lista2) //creamos función para los caminos 
        {
        if (!arbol.esHoja()) //si no es una hoja
        {
            lista1.Add(arbol.getDatoRaiz().texto);//agregamos el texto del nodo raiz a la lista
            foreach(ArbolGeneral<DatoDistancia> hijo in arbol.getHijos()) //hacemos una busqueda sobre cada hijo 
            {
                Busqueda_de_caminos(hijo, lista1, lista2); //se llama recursivamente a la función con el hijo
                lista1.RemoveAt(lista2.Count - 1); //se elimina el último elemento de lista1. Esto se hace para retroceder y explorar otros caminos posibles desde el mismo nivel del árbol.
            }
        }
        else //si es una hoja
        {
            lista1.Add(arbol.getDatoRaiz().texto); //agregamos el texto del nodo
            ArrayList auxiliar = new ArrayList(); //creamos una lista auxiliar
            foreach (string ele in lista1) //iteramos en la lista1 que contiene los strings
            {
                auxiliar.Add(ele); //se agrega a la lista auxiliar los strings de lista1
            }
            lista2.Add(auxiliar); //se agrega a la lista2 toda la lista auxiliar
        }
        }
		public String Consulta2(ArbolGeneral<DatoDistancia> arbol)
		{
		int contador = 1; //utilizamos un contador y lo inicializamos en 1
            ArrayList lista1 = new ArrayList(); //creamos una lista llamada lista1
            ArrayList lista2 = new ArrayList(); //creamos una lista llamada lista2
            string palabra1 = "camino: "; //creo el principio
            string palabra2 = ""; //creo una oracion donde se van a guardar el texto de cada nodo hacia las hojas
            Busqueda_de_caminos(arbol, lista1, lista2); //utilizo la función de busqueda de caminos
            foreach (ArrayList lista in lista2) //recorremos todas las listas dentro de lista2
            {
                palabra2 = palabra2 + " lista N°" + contador + " "; //agregamos el numero de contador a la palabra2
                foreach (string nodo in lista) //recorremos cada nodo de la lista
                {
                    palabra2 = palabra2 + nodo + " , "; //agregamos el texto del nodo seguido de una coma y un espacio
                }
                palabra2 = palabra2 + "///"; //agregamos separadores
                contador++; //aumentamos contador
            }
            palabra1 = palabra1 + palabra2; //concatenamos
            return palabra1; //retorna oracion
        }
}

/*         public void Busqueda_de_caminos(ArbolGeneral<DatoDistancia> arbol, ArrayList listas, ArrayList strings) FUNCIONA
        {
            if (arbol.esHoja())
            {
                strings.Add(arbol.getDatoRaiz().texto);
                ArrayList aux = new ArrayList();
                foreach (string ele in strings)
                {
                    aux.Add(ele);
                }
                listas.Add(aux);

            }
            else
            {
                strings.Add(arbol.getDatoRaiz().texto);
                foreach (ArbolGeneral<DatoDistancia> hijo in arbol.getHijos())
                {
                    Busqueda_de_caminos(hijo, listas, strings);
                    strings.RemoveAt(strings.Count - 1);
                }
            } */
            /*
        }
        public String Consulta2(ArbolGeneral<DatoDistancia> arbol) //PUNTO 4
        {
            int contador = 0;
            string oracion1 = "camino hacia las hojas"; //creo oracion inial
            string oracion = ""; //creo una oracion donde se van a guardar el texto de cada nodo hacia las hojas
            ArrayList lista1 = new ArrayList();
            ArrayList lista2 = new ArrayList(); //string
            Busqueda_de_caminos(arbol, lista1, lista2);
            foreach (ArrayList lista in lista1)
            {
                oracion = oracion + " LISTA: " + contador + " ";
                foreach (string nodo in lista)
                {

                    oracion = oracion + nodo + " , ";
                }
                oracion = oracion + "///";
                contador++;
            }
            oracion1 += oracion;
            return oracion1; //retorna oracion */

            /*public String Consulta3(ArbolGeneral<DatoDistancia> arbol) 
		{
		string oracion1 = "Esta es la oracion que se formo: "; //creo el principio 
        string oracion2 = ""; //creo una variable donde se va a guardar el texto de cada nodo hacia las hojas
        DatoDistancia auxiliar = new DatoDistancia(1, "Texto", "Descripción"); //creo un objeto DatoDistancia 
        ArbolGeneral<DatoDistancia> contador = new ArbolGeneral<DatoDistancia>(auxiliar); //arbolgeneral que contiene auxiliar
        int nivel = 0; //contador de niveles, inicializado en -1 para que al menos haga la iteracion en el nivel 0
        if (arbol.esHoja()) //pregunto si es hoja
        {
            oracion2 = oracion2 + "nivel: " + nivel + " ,Distancia: " + arbol.getDatoRaiz().distancia + " ,Texto: " + arbol.getDatoRaiz().texto + " ,Descripcion: " + arbol.getDatoRaiz().descripcion; //concateno la oracion con los datos del DatoDistancia
            nivel++; //subo el nivel para que quede en 0
        }
        else//si no es
        {
            
            Cola<ArbolGeneral<DatoDistancia>> cola = new Cola<ArbolGeneral<DatoDistancia>>();  //creo una cola
            ArbolGeneral<DatoDistancia> arbolaux;  // creo un arbol auxiliar que servira de puntero
            cola.encolar(arbol); //encolo el arbol pasado como parametro
            cola.encolar(contador); //encolo el flag
            nivel++; //subo el nivel
            while (!cola.esVacia()) //mientras la cola no este vacia
            {
                arbolaux = cola.desencolar(); //desencolo el primer elemento de la cola
                if (arbolaux != contador) //comparo si es distinto al flag
                {
                    oracion2 = oracion2 + " Nivel: " + nivel + " ,Distancia: " + arbolaux.getDatoRaiz().distancia + " ,Texto: " + arbolaux.getDatoRaiz().texto + " ,Descripcion: " + arbolaux.getDatoRaiz().descripcion; //concateno la oracion con los datos del DatoDistancia
                    foreach (var hijo in arbolaux.getHijos())
                    { //pido la lista de hijos
                        cola.encolar(hijo); //los encolo
                    }
                }
                else
                { //si es igual al flag 
                    nivel++; //subo el nivel
                    if (!cola.esVacia())
                    { //pregunto si la cola esta vacia
                        cola.encolar(contador); //encolo un flag
                    }
                }
            }
        }
        string oracion = oracion1 + oracion2; //concateno las oraciones
        return oracion; //retorno las oraciones*/
		}