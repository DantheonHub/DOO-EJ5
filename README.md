# DOO EJ5
En este ejercicio he seguido el enunciado, pero he hecho algunas modificaciones en el diseño porque no me convencía la estructura propuesta. Por ejemplo, la clase ProcesarDatos parecía orientada a manejar bases de datos de una máquina virtual ya existente, pero la utilicé para interactuar con la máquina y sus bases de datos directamente, ya que me pareció más apropiado para el contexto.

En cuanto a la clonación de bases de datos, decidí implementarla dentro de ProcesarDatos, tomando los datos de la máquina virtual asociada. Aunque no se especificaba cómo hacerlo, pensé que esta forma tendría más sentido.

Me desvíe un poco de la propuesta original, pero traté de mantener la funcionalidad central. Aún así, creo que el diseño podría mejorarse si separamos mejor las responsabilidades de cada clase, evitando que interactúen tan directamente entre sí. Este desacoplamiento permitiría hacer el sistema más modular y escalable.
