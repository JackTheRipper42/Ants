function init()
	ant.sugarEnterView = sugarEnterView;
	ant.appleEnterView = appleEnterView;
	ant.reachSugar = reachSugar;
	ant.reachApple = reachApple;
	ant.reachAnthill = reachAnthill;
	ant.reachDestination = reachDestination;
	ant.reachMark = reachMark;

	ant.setDestination(100, math.random(-10,10));
	waypoints = 1;
	knownSugar = nil;
	hasTarget = false;
	followsSugarMarker = false;
end

function sugarEnterView(sugar)
	if not ant.isCarrying and not hasTarget then
		ant.setDestinationGlobal(sugar.position.x, sugar.position.y);
		hasTarget = true;
	end
	if not ant.isCarrying and followsSugarMarker then
		ant.setDestinationGlobal(sugar.position.x, sugar.position.y);
		hasTarget = true;
		followsSugarMarker = false;
	end
end

function appleEnterView(apple)
	if not ant.isCarrying and not hasTarget and not followsSugarMarker then
		if apple.carryingAnts < 10 then
			ant.setDestinationGlobal(apple.position.x, apple.position.y);
			hasTarget = true;
		end
	end
end

function reachMark(mark)
	if not ant.isCarrying and not hasTarget then
		if mark.information.isSugar == true then
			--ant.setDestinationGlobal(mark.position.x, mark.position.y);
			--print("values:");
			--print(mark.direction);
			--print(ant.rotation);
			ant.setDestination(100, mark.direction);
			hasTarget = true;
			followsSugarMarker = true;
		end
	end
end

function reachSugar(sugar)
	ant.mark(100, { isSugar = true });
	knownSugar = sugar;
	ant.pickSugar();
	if not ant.isCarrying then
		print("fuck");
	end
	ant.goToAnthill();
end

function reachApple(apple)
	if apple.carryingAnts < 10 then
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
	hasTarget = false;
	if waypoints <= 3 then
		ant.setDestination(40, math.random(75, 105));
		waypoints = waypoints + 1;
	else
		ant.goToAnthill();
	end
end

function reachAnthill()
	waypoints = 1;
	hasTarget = false;
	if not knownSugar == nil then
		if knownSugar.amount <= 0 then
			knownSugar = nil;
		end		
	end
	if knownSugar == nil then
		ant.setDestination(20, math.random(150, 175));
	else
		ant.setDestinationGlobal(knownSugar.position.x, knownSugar.position.y);
		hasTarget = true;
	end
end
