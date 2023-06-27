
using System;
using System.Collections.Generic;
using tp1;
using System.Collections;

namespace tpfinal
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

		public String Consulta1(ArbolGeneral<DatoDistancia> arbol)
		{
        string palabra = ""; // Creo una oración donde voy a agrupar los textos de cada nodo
        if (arbol.esHoja()) //si el arbol es una hoja
        {
            palabra = palabra + " " + arbol.getDatoRaiz().texto; // Asigna el texto de la raíz a palabra_2
        }
        else //si no es hoja
        {
            foreach (ArbolGeneral<DatoDistancia> hijo in arbol.getHijos()) //se itera sobre cada hijo del arbol
            {
                palabra = palabra + Consulta1(hijo); // Concatena los resultados de cada hijo recursivamente
            }
        }
        return palabra; // por ultimo retorno la oración
        }


        public void Busqueda_de_caminos(ArbolGeneral<DatoDistancia> arbol, ArrayList listas, ArrayList strings) //creamos función para los caminos 
        {
        if (!arbol.esHoja()) //si no es una hoja
        {
            strings.Add(arbol.getDatoRaiz().texto); //agregamos el texto del nodo raiz a la lista de strings
            foreach (ArbolGeneral<DatoDistancia> hijo in arbol.getHijos()) //hacemos una busqueda sobre cada hijo 
            {
                Busqueda_de_caminos(hijo, listas, strings);  //se llama recursivamente a la función con el hijo
                strings.RemoveAt(strings.Count - 1); //se elimina el último elemento de lista1. Esto se hace para retroceder 
                //y explorar otros caminos posibles desde el mismo nivel del árbol.
            }
        }
        else  //si es una hoja
        {
            strings.Add(arbol.getDatoRaiz().texto);  //agregamos el texto del nodo a la lista de strings
            ArrayList lista_auxiliar = new ArrayList();  //creamos una lista auxiliar
            foreach (string ele in strings) //iteramos en la lista de los strings
            {
                lista_auxiliar.Add(ele); //se agrega a la lista auxiliar los strings de lista de strings
            }
            listas.Add(lista_auxiliar); //se agrega a la lista2 toda la lista auxiliar
        }
        }
        public String Consulta2(ArbolGeneral<DatoDistancia> arbol)
        {
            int contador = 1; //utilizamos un contador y lo inicializamos en 1
            ArrayList lista1 = new ArrayList(); //creamos una lista llamada lista1
            ArrayList lista2 = new ArrayList(); //creamos una lista llamada lista2
            string palabra = ""; //creo una variable donde se van a guardar el texto de cada nodo hacia las hojas
            Busqueda_de_caminos(arbol, lista1, lista2); //utilizo la función de busqueda de caminos
            foreach (ArrayList lista in lista1) //recorremos todas las listas dentro de lista2
            {
                palabra = palabra + " Camino N°" + contador + " :"; //agregamos el numero de contador a la palabra2
                foreach (string nodo in lista)  //recorremos cada nodo de la lista
                {
                    palabra = palabra + nodo + ", "; //agregamos el texto del nodo seguido de una coma y un espacio
                }
                palabra = palabra + "-"; //agregamos separadores
                contador++; //aumentamos contador
            }
            return palabra; //retorna oracion
        }
		public String Consulta3(ArbolGeneral<DatoDistancia> arbol) 
		{
        string palabras = ""; //creo una variable donde se va a guardar el texto de cada nodo hacia las hojas
        DatoDistancia auxiliar = new DatoDistancia(1, "Texto", "Descripción"); //creo un objeto DatoDistancia 
        ArbolGeneral<DatoDistancia> contador = new ArbolGeneral<DatoDistancia>(auxiliar); //arbolgeneral que contiene auxiliar
        int nivel = -1; //contador de niveles, inicializado en -1 para que al menos haga la iteracion en el nivel 0
        if (arbol.esHoja()) //si es una hoja 
        {
            nivel= nivel+1; //subo el nivel para que quede en 0
            palabras = palabras + "El nivel es: " + nivel +". Texto: " + arbol.getDatoRaiz().texto + " ,Descripcion: " + arbol.getDatoRaiz().descripcion + "\n"; //agregamos la información
        }
        else//si no es
        {
            nivel=nivel+1; //subo el nivel
            Cola<ArbolGeneral<DatoDistancia>> cola = new Cola<ArbolGeneral<DatoDistancia>>();  //creo una cola para almacenar nodo
            ArbolGeneral<DatoDistancia> arbol_auxiliar;  // creo un arbol auxiliar que servira de puntero
            cola.encolar(arbol); //encolo el arbol pasado como parametro
            cola.encolar(contador); //encolo el contador
            while (!cola.esVacia()) //mientras la cola no este vacia
            {
                arbol_auxiliar = cola.desencolar(); //desencolo el primer elemento de la cola en el arbol
                if (arbol_auxiliar != contador) //mientras sea distinto a contador
                {
                    palabras = palabras + " El nivel es: " + nivel +". Texto: " + arbol_auxiliar.getDatoRaiz().texto + " ,Descripcion: " + arbol_auxiliar.getDatoRaiz().descripcion + " \n";//agregamos la información
                    foreach (var hijo in arbol_auxiliar.getHijos()) //recorro todos los hijos del nodo utilizando un foreach
                    {
                        cola.encolar(hijo); //los encolo
                    }
                }
                else //si es igual al contador
                { 
                    nivel =nivel+1; //subo el nivel
                    if (!cola.esVacia())//si la cola esta vacia
                    { 
                        cola.encolar(contador); //encolo el contador
                    }
                }
            }
        }
        return palabras; //retorno las oraciones
        }

		public void AgregarDato(ArbolGeneral<DatoDistancia> arbol, DatoDistancia dato)
		{
		Cola<ArbolGeneral<DatoDistancia>> cola = new Cola<ArbolGeneral<DatoDistancia>>(); //creacion de la cola para recorrer el arbol general por niveles
        ArbolGeneral<DatoDistancia> auxiliar; //creamos arbol auxiliar
        cola.encolar(arbol); //encolamos el arbol que se paso de parametro
        int distancia = CalcularDistancia(arbol.getDatoRaiz().texto, dato.texto); //creo variable que calcule la distancia entre la palabra de la raiz y el dato 
        if (distancia != arbol.getDatoRaiz().distancia) // si la distancia es diferente al dato raiz del arbol 
        {
            ArbolGeneral<DatoDistancia> nuevo = new ArbolGeneral<DatoDistancia>(dato); //se crea un arbol general con el DatoDistancia
            arbol.agregarHijo(nuevo); //se agrega ese arbol al arbol que se paso como parametro
        }
        while (!cola.esVacia()) //mientras la cola no este vacia se ejecuta lo de dentro
        {
            auxiliar = cola.desencolar(); //se desencola un árbol de la cola y se asigna a el arbol aux
            foreach (var hijo in auxiliar.getHijos()) // hijos del arbol auxiliar
            { 
                cola.encolar(hijo); //y los encolo
            }
            if (distancia == auxiliar.getDatoRaiz().distancia) // si la distancia calculada es igual a la distancia del dato raíz del árbol auxiliar
            {
                AgregarDato(auxiliar, dato); // Si son iguales, se llama recursivamente a la función AgregarDato,con parámetros el árbol aux y el objeto dato. 
                //profundidad en el árbol para encontrar un nodo con la misma distancia, y se agregará el dato como hijo de ese nodo.
            }
        }
        
		}
        public void Buscar(ArbolGeneral<DatoDistancia> arbol, string elementoABuscar, int umbral, List<DatoDistancia> collected)
        {
            if (arbol == null) //si no hay arbol
            return; //retorna

        int distancia = CalcularDistancia(arbol.getDatoRaiz().texto, elementoABuscar); //calculamos la distancia entre la raiz y el dato que buscamos

        if (distancia <= umbral) //si el umbras es igual o mas grande que la distancia, se agrega a la lista
            collected.Add(arbol.getDatoRaiz());

        foreach (var hijo in arbol.getHijos()) //si no es asi, se busca a los hijos
        {
            Buscar(hijo, elementoABuscar, umbral, collected); //se llama recursivamente a la función Buscar con hijo, para agregarlos todos
        }
        }
    }
}