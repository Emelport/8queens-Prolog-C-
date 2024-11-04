:- dynamic solutions/1.  % Definimos soluciones como dinámico para almacenar soluciones

:- initialization(main).

% Punto de inicio del programa
main :-
    solve,
    consult_solutions,  % Llamamos a la función para consultar las soluciones
    halt.

% Función para resolver el problema de las 8 reinas
solve :-
    findall(Solution, (perm([1,2,3,4,5,6,7,8], Solution), valid_solution(Solution)), Solutions),
    retractall(solutions(_)),  % Limpiamos soluciones anteriores
    assert(solutions(Solutions)),  % Almacenamos las nuevas soluciones
    length(Solutions, Count),
    write('Numero de soluciones únicas encontradas: '), write(Count), nl.

% Consultar soluciones
consult_solutions :-
    solutions(Solutions),
    print_all_solutions(Solutions).

% Imprime todas las soluciones en tableros
print_all_solutions([]).
print_all_solutions([Solution|Solutions]) :-
    print_board(Solution),
    nl,  % Espacio entre soluciones
    print_all_solutions(Solutions).

% Verifica si una solución es válida
valid_solution(Perm) :-
    \+ (nth1(I1, Perm, X1), nth1(I2, Perm, X2), I1 < I2, abs(X1 - X2) =:= abs(I1 - I2)).

% Imprime el tablero para una solución
print_board(Solution) :-
    write('Tablero:\n'),
    print_rows(Solution).

% Imprime las filas del tablero
print_rows([]).
print_rows([H|T]) :-
    print_row(H),
    nl,
    print_rows(T).

% Imprime una fila del tablero
print_row(Row) :-
    findall(Cell, (between(1, 8, Pos), (Pos = Row -> Cell = '♛' ; Cell = '.')), Cells),
    atomic_list_concat(Cells, ' ', RowString),
    write(RowString).

% Permutación
perm([], []).
perm(L, [H|P]) :-
    select(H, L, R),
    perm(R, P).
