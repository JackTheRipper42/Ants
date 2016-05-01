namespace Assets.Scripts.Lua
{
    public class SugarTable : EntityTable
    {
        private readonly Sugar _sugar;

        public SugarTable(Sugar sugar)
            : base(sugar)
        {
            _sugar = sugar;
        }

        public int getAmount()
        {
            return _sugar.Capacity;
        }
    }
}
