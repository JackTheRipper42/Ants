function init()
	ant.update = update;
	ant.sugarEnterView = sugarEnterView;
	ant.appleEnterView = appleEnterView;
	
	ant.setDestination(20, 0);
	hasTarget = false;
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
