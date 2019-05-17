print("Objs2Spawn is : "..Objs2Spawn)
var2Read = 42
varTable = {1,2,3,4,5}
varTable.default = 1
varTable.map = {}
varTable.map.name = 'map'

meta = {name = 'meata'}
setmetatable(varTable, meta)

function TestFunc(strs)
	print("get func by variable")
end

function PrintName()
	print(varTable.map.name);
end
