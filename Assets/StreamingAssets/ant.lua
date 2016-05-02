function init()
	ant.sugarEnterView = sugarEnterView;
	ant.appleEnterView = appleEnterView;
	ant.reachSugar = reachSugar;
	ant.reachApple = reachApple;
	ant.reachAnthill = reachAnthill;
	ant.reachDestination = reachDestination;
	
	ant.setDestination(20, 0);
	waypoints = 1;
	knownSugar = nil;
end

function sugarEnterView(sugar)
	if not ant.isCarrying then
		ant.setDestinationGlobal(sugar.position.x, sugar.position.y);
	end
end

function appleEnterView(apple)
	if not ant.isCarrying then
		if apple.carryingAnts < 5 then
			ant.setDestinationGlobal(apple.position.x, apple.position.y);
		end
	end
end

function reachSugar(sugar)
	knownSugar = sugar;
	ant.pickSugar();
	if not ant.isCarrying then
		print("fuck");
	end
	ant.goToAnthill();
end

function reachApple(apple)
	if apple.carryingAnts < 5 then
		ant.pickApple();
		if not ant.isCarrying then 
			print("fuck")
		end
		ant.goToAnthill();
	else
		reachDestination();
	end
end

function reachDestination()
	if waypoints <= 3 then
		ant.setDestination(20, 90);
		waypoints = waypoints + 1;
	else
		ant.goToAnthill();
	end
end

function reachAnthill()
	if not knownSugar == nil then
		if knownSugar.amount <= 0 then
			knownSugar = nil;
		end		
	end
	if knownSugar == nil then
		ant.setDestination(20, 165);
	else
		ant.setDestinationGlobal(knownSugar.position.x, knownSugar.position.y);
	end
	waypoints = 1;
end
