function init()
	ant.update = update;
	ant.sugarEnterView = sugarEnterView;
	ant.appleEnterView = appleEnterView;
	ant.reachSugar = reachSugar;
	ant.reachApple = reachApple;
	ant.reachAnthill = reachAnthill;
	ant.reachDestination = reachDestination;
	
	ant.setDestination(20, 0);
	hasTarget = false;
	waypoints = 1;
end

function update()
end

function sugarEnterView(sugar)
	if not hasTarget then
		ant.setDestinationGlobal(sugar.position.x, sugar.position.y);
		hasTarget = true;
	end
end

function appleEnterView(apple)
	if not hasTarget then
		ant.setDestinationGlobal(apple.position.x, apple.position.y);
		hasTarget = true;
	end
end

function reachSugar(sugar)
	ant.pickSugar();
	if not ant.isCarrying then
		print("fuck");
	end
	ant.goToAnthill();
end

function reachApple(apple)
	ant.pickApple();
	if not ant.isCarrying then 
		print("fuck")
	end
	ant.goToAnthill();
end

function reachDestination()
	print(waypoints);
	if waypoints <= 3 then
		print("turn");
		ant.setDestination(20, 90);
		waypoints = waypoints + 1;
	else
	print("go back");
		ant.goToAnthill();
	end
end

function reachAnthill()
	ant.setDestination(20, 165);
	waypoints = 1;
	hasTarget = false;
end
