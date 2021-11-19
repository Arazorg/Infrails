public interface IDebuff
{
    void AcceptWithoutDebufff(IDebuffVisitor debuffVisitor);

    void Accept(IDebuffVisitor debuffVisitor);
}
