story(1)
{
};
story(2)
{
      onmessage("start")
      {
	sendmessage("Cube","GetValue",10);
	sendmessage("Cube","GetValue",30)
      }
};